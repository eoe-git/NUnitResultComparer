# NUnitResultComparer
Compare the results of two NUnit result files

##Setup
All of the settings are in the app.config

\<key>: Description (\<value>)

###Mandatory
CurrentResult: (Enter the full path to where your current results are located)

PreviousResult: (Enter the full path to where your previous results are located)

ComparisonResultPath: (Enter the location where you want the comparison results)

###Optional
ComparisonView: Enter what type of output you want (all/txt/csv/cmd)

ShowChangedResultsOnly: Enter if you only want results that changed (true/false)

ShowTotals: Enter if you want to see a header with totals (true/false)

IsNUnit3: Enter if you are using NUnit3 (true/false)

####Ranking Results
Possible results are Passed, Inconclusive, Failed, Error, NotRunnable, Cancelled, Ignored, Explicit, Skipped

Result: (positive integer value)

You can set which results are better or worse than one another

The higher the value the better the result is (Passed must be the highest value)

Values can be the same for different results

Set the result to 0 if you want any tests with that result to be ignored


