import pandas as pd
import csv

for i in range (1, 13):   
    pre = str("Keys") + str(i) + str(".txt")
    after = str("Keys") + str(i) + str(".csv")
    pd.read_fwf(pre).to_csv(after)
    