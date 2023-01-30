using DeploymentToolkit.Scripting.Exceptions;
using DeploymentToolkit.Scripting.Modals;
using NLog;
using System;
using System.Collections.Generic;

namespace DeploymentToolkit.Scripting
{
    public static class Evaluation
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static bool Evaluate(string condition)
        {
            ProcessGroup(condition, out var mainGroup);

            return mainGroup.IsTrue();
        }

        private static int ProcessGroup(string condition, out Group group, Group parentGroup = null)
        {
            _logger.Trace($"Starting processing of '{condition}'");
#if DEBUG && EVALUTATOR_TRACE
            Debug.WriteLine($"Starting processing of '{condition}'");
#endif
            group = new Group();

            var endOfThisGroup = 0;
            var toCut = new List<int[]>();
            if(parentGroup != null)
            {
                parentGroup.SubGroups.Add(group);
            }

            if(condition.StartsWith("("))
            {
                condition = condition.Substring(1, condition.Length - 1);
            }

            var lastGroup = default(Group);
            var currentGroup = default(Group);
            var currentCondition = LinkType.None;

            for(var i = 0; i < condition.Length; i++)
            {
                var currentCharacter = condition[i];
#if DEBUG && EVALUTATOR_TRACE
                Debug.WriteLine($"{currentCharacter}");
#endif
                if(currentCharacter == ')')
                {
                    // End of this group
                    endOfThisGroup = i;
#if DEBUG && EVALUTATOR_TRACE
                    Debug.WriteLine($"Group ends at {i}");
#endif
                    break;
                }
                else if(currentCharacter == '(')
                {
#if DEBUG && EVALUTATOR_TRACE
                    Debug.WriteLine($"Start of another group at {i}");
#endif
                    lastGroup = currentGroup;
                    // Another sub-group
                    var groupStart = i;
                    var groupEnd = i + ProcessGroup(condition.Substring(i, condition.Length - i), out currentGroup, group) + 1;

                    toCut.Add(new int[] { groupStart, groupEnd });

                    i = groupEnd; // Skip to the end of the group
#if DEBUG && EVALUTATOR_TRACE
                    Debug.WriteLine($"Skipping to {i}");
#endif

                    if(currentCondition != LinkType.None)
                    {
                        if(lastGroup == null || currentGroup == null)
                        {
                            throw new ScriptingInvalidConditionException("Invalid placed AND or OR");
                        }

                        group.GroupLinks.Add(new GroupLink(lastGroup, currentCondition, currentGroup));
                        currentCondition = LinkType.None;
                    }

                    continue;
                }
                else if(currentCharacter == 'A')
                {
                    var andOperator = condition.Substring(i, 3);
                    if(andOperator == "And")
                    {
                        currentCondition = LinkType.And;
                        toCut.Add(new int[] { i, i + 2 });
                        i += 2;
#if DEBUG && EVALUTATOR_TRACE
                        Debug.WriteLine("Detected AND");
#endif
                    }

                    continue;
                }
                else if(currentCharacter == 'O')
                {
                    var orOperator = condition.Substring(i, 2);
                    if(orOperator == "Or")
                    {
                        currentCondition = LinkType.Or;
                        toCut.Add(new int[] { i, i + 1 });
                        i += 1;
#if DEBUG && EVALUTATOR_TRACE
                        Debug.WriteLine("Detected OR");
#endif
                    }

                    continue;
                }
            }

            toCut.Reverse();
#if DEBUG && EVALUTATOR_TRACE
            Debug.WriteLine($"Condition: {condition}");
            Debug.WriteLine($"Len: {condition.Length}");
            Debug.WriteLine($"EndOfThisGroup: {endOfThisGroup}");
#endif

            if(parentGroup == null)
            {
                // If we are the main group then there can't be text after this group closes
                if(endOfThisGroup != condition.Length - 1)
                {
                    throw new ScriptingInvalidConditionException("Leftover text after group ended");
                }
            }

            var toEvaluate = condition;
            if(endOfThisGroup < condition.Length - 1)
            {
                toEvaluate = condition.Substring(0, endOfThisGroup);
            }

            foreach(var cut in toCut)
            {
                toEvaluate = toEvaluate.Substring(0, cut[0]) + toEvaluate.Substring(cut[1] + 1, toEvaluate.Length - cut[1] - 1);
            }

            if(toEvaluate.EndsWith(")"))
            {
                toEvaluate = toEvaluate.Substring(0, toEvaluate.Length - 1);
            }

            toEvaluate = toEvaluate.Trim();

            if(!string.IsNullOrEmpty(toEvaluate))
            {
                group.Condition = EvaluateCondition(toEvaluate);
            }
            else if(group.SubGroups.Count == 0)
            {
                throw new ScriptingInvalidGroupException("A group needs to have exactly one conditon or multiple sub-conditions");
            }

            return endOfThisGroup;
        }

        private static Condition EvaluateCondition(string condition)
        {
            _logger.Trace($"Evaluating {condition}");
#if DEBUG && EVALUTATOR_TRACE
            Debug.WriteLine($"Evaluating {condition}");
#endif

            var currentIndex = 0;
            var result = new Condition();
            do
            {
                if(currentIndex >= condition.Length)
                {
                    throw new ScriptingInvalidConditionException($"Invalid condtion {condition}");
                }

                var currentCharacter = condition[currentIndex++];

                if(currentCharacter == ' ')
                {
                    continue;
                }
                else if(currentCharacter == '\'')
                {
                    var stringEnd = condition.Substring(currentIndex, condition.Length - currentIndex).IndexOf('\'');
                    if(stringEnd == -1)
                    {
                        throw new ScriptingInvalidStringException($"Invalid or incomplete string: {condition.Substring(currentIndex, condition.Length - currentIndex)}");
                    }

                    var name = condition.Substring(currentIndex, stringEnd);
                    currentIndex += stringEnd + 1;

                    if(string.IsNullOrEmpty(result.FirstString))
                    {
                        result.FirstString = name;
                    }
                    else if(string.IsNullOrEmpty(result.SecondString))
                    {
                        result.SecondString = name;
                    }
                    else
                    {
                        throw new ScriptingInvalidConditionException($"Invalid condition. Got multiple variables. ({condition})");
                    }

                    continue;
                }
                else if(
                    currentCharacter == '=' ||
                    currentCharacter == '!'
                )
                {
                    var nextCharacter = condition[currentIndex];

                    if(nextCharacter == '=')
                    {
                        // == or !=
                        result.Operator = GetOperatorFromString($"{currentCharacter}{nextCharacter}");
                        currentIndex += 2;
                    }
                    else
                    {
                        throw new ScriptingInvalidOperatorException($"Invalid operator {currentCharacter}{nextCharacter}");
                    }

                    continue;
                }
                else if(
                    currentCharacter == '>' ||
                    currentCharacter == '<'
                )
                {
                    var nextCharacter = condition[currentIndex];
                    if(nextCharacter == '=')
                    {
                        result.Operator = GetOperatorFromString($"{currentCharacter}{nextCharacter}");
                        currentIndex += 2;
                    }
                    else
                    {
                        result.Operator = GetOperatorFromString($"{currentCharacter}");
                        currentIndex += 1;
                    }
                }
            }
            while(currentIndex < condition.Length);
#if DEBUG && EVALUTATOR_TRACE
            Debug.WriteLine($"Final condition: '{result.FirstString}' {result.Operator} '{result.SecondString}' as {result.CompareType} -> {result.IsTrue()}");
#endif

            return result;
        }

        private static Operator GetOperatorFromString(string input)
        {
#if DEBUG && EVALUTATOR_TRACE
            Debug.WriteLine($"Processing {input}");
#endif
            switch(input)
            {
                case "==":
                    return Operator.Equal;
                case "!=":
                    return Operator.NotEqual;
                case ">":
                    return Operator.Greater;
                case ">=":
                    return Operator.GreaterEqual;
                case "<":
                    return Operator.Less;
                case "<=":
                    return Operator.LessEqual;
            }

            throw new ArgumentOutOfRangeException(nameof(input), "Invalid operator found!");
        }
    }
}
