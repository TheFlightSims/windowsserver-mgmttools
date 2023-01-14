// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.CollectionWrapper`1
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class CollectionWrapper<T> : 
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable,
    IWrappedCollection,
    IList,
    ICollection
  {
    private readonly IList? _list;
    private readonly ICollection<T>? _genericCollection;
    private object? _syncRoot;

    public CollectionWrapper(IList list)
    {
      ValidationUtils.ArgumentNotNull((object) list, nameof (list));
      if (list is ICollection<T> objs)
        this._genericCollection = objs;
      else
        this._list = list;
    }

    public CollectionWrapper(ICollection<T> list)
    {
      ValidationUtils.ArgumentNotNull((object) list, nameof (list));
      this._genericCollection = list;
    }

    public virtual void Add(T item)
    {
      if (this._genericCollection != null)
        this._genericCollection.Add(item);
      else
        this._list.Add((object) item);
    }

    public virtual void Clear()
    {
      if (this._genericCollection != null)
        this._genericCollection.Clear();
      else
        this._list.Clear();
    }

    public virtual bool Contains(T item) => this._genericCollection != null ? this._genericCollection.Contains(item) : this._list.Contains((object) item);

    public virtual void CopyTo(T[] array, int arrayIndex)
    {
      if (this._genericCollection != null)
        this._genericCollection.CopyTo(array, arrayIndex);
      else
        this._list.CopyTo((Array) array, arrayIndex);
    }

    public virtual int Count => this._genericCollection != null ? this._genericCollection.Count : this._list.Count;

    public virtual bool IsReadOnly => this._genericCollection != null ? this._genericCollection.IsReadOnly : this._list.IsReadOnly;

    public virtual bool Remove(T item)
    {
      if (this._genericCollection != null)
        return this._genericCollection.Remove(item);
      int num = this._list.Contains((object) item) ? 1 : 0;
      if (num == 0)
        return num != 0;
      this._list.Remove((object) item);
      return num != 0;
    }

    public virtual IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) this._genericCollection ?? this._list.Cast<T>()).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this._genericCollection ?? (IEnumerable) this._list).GetEnumerator();

    int IList.Add(object value)
    {
      CollectionWrapper<T>.VerifyValueType(value);
      this.Add((T) value);
      return this.Count - 1;
    }

    bool IList.Contains(object value) => CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T) value);

    int IList.IndexOf(object value)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support IndexOf.");
      return CollectionWrapper<T>.IsCompatibleObject(value) ? this._list.IndexOf((object) (T) value) : -1;
    }

    void IList.RemoveAt(int index)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support RemoveAt.");
      this._list.RemoveAt(index);
    }

    void IList.Insert(int index, object value)
    {
      if (this._genericCollection != null)
        throw new InvalidOperationException("Wrapped ICollection<T> does not support Insert.");
      CollectionWrapper<T>.VerifyValueType(value);
      this._list.Insert(index, (object) (T) value);
    }

    bool IList.IsFixedSize => this._genericCollection != null ? this._genericCollection.IsReadOnly : this._list.IsFixedSize;

    void IList.Remove(object value)
    {
      if (!CollectionWrapper<T>.IsCompatibleObject(value))
        return;
      this.Remove((T) value);
    }

    object IList.this[int index]
    {
      get
      {
        if (this._genericCollection != null)
          throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
        return this._list[index];
      }
      set
      {
        if (this._genericCollection != null)
          throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
        CollectionWrapper<T>.VerifyValueType(value);
        this._list[index] = (object) (T) value;
      }
    }

    void ICollection.CopyTo(Array array, int arrayIndex) => this.CopyTo((T[]) array, arrayIndex);

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    private static void VerifyValueType(object value)
    {
      if (!CollectionWrapper<T>.IsCompatibleObject(value))
        throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, value, (object) typeof (T)), nameof (value));
    }

    private static bool IsCompatibleObject(object value) => value is T || value == null && (!typeof (T).IsValueType() || ReflectionUtils.IsNullableType(typeof (T)));

    public object UnderlyingCollection => (object) this._genericCollection ?? (object) this._list;
  }
}
