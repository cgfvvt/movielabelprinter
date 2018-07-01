Movie Label Printer
Test Task

by <cgfvvt> (c) 2018

Implementation took around 15 hours total (from reading requirements till code deployed and
notification posted)


1. Original Requirements
------------------------

The task
Create a windows form application:

Allow user to select from list of installed printers

Allow the user to enter a movie id.

Allow the user to Print a label to the selected printer

When the user clicks on a button to print a movie label:

Download the movie from:
"https://tup-ops-eng-coding-exercise.s3.amazonaws.com/data/{0}.json"
replace {0} with the movie id.
Print the label
What we are looking for:

Object oriented design and programming experience
Code Quality
Exception handling
Test coverage
Working example video:

See video at: http://recordit.co/rmYUMASnoo

Sample Movie labels files:

See fonts used at: https://tup-ops-eng-coding-exercise.s3.amazonaws.com/labels/movie_fonts.oxps

See printed files at:

https://tup-ops-eng-coding-exercise.s3.amazonaws.com/labels/movie1.oxps
https://tup-ops-eng-coding-exercise.s3.amazonaws.com/labels/movie2.oxps
https://tup-ops-eng-coding-exercise.s3.amazonaws.com/labels/movie3.oxps
https://tup-ops-eng-coding-exercise.s3.amazonaws.com/labels/movie4.oxps
https://tup-ops-eng-coding-exercise.s3.amazonaws.com/labels/movie5.oxps
https://tup-ops-eng-coding-exercise.s3.amazonaws.com/labels/movie6.oxps

2. General Description
----------------------

The task was implemented as a .Net Windows Forms application with supporting class libraries.

Following assumptions were made:

2.1.  The application is intended for use by single person occasionally printing labels to
      stick them on DVD boxes or other packages
2.2.  Label should contain human readable information only
2.3.  Target printers should most likely be label printers
2.4.  Text on label should occupy as much place as reasonably possible
2.5.  No assumptions can be made about data types returned in JSONs from web server; thus they
      all considered of type string
2.6.  Testing should be done only on range of movie IDs 1-6, as only those seem to be available
2.7.  In case if other movie IDs will produce extra properties in JSON, they should be silently
      ignored
2.8.  Font family is hard-coded in application, and it is "Serif" family; this approach seem to
      work reliably on Intermec and Zebra printers
2.9.  Font size absolute values should be limited by range from 4 to 144
2.10. Application should use default printer setting for specified printer, including paper
      size and orientation
2.11. Implementation should not take more than 2 days

3. Testing
----------

Positive testing covered following parameter ranges (but not all possible combinations of them):

3.1. Paper size: A8 - A4
3.2. Movie Id: 1-6, also 7 and non-numeric text for negative testing
3.3. Page layout: Portrait and Landscape
3.4. Printers or emulators: PDFCreator 1.4.1, XPS Writer, Xerox Phaser 3122
3.5. Operating System: Windows 7 SP 1 x64

Only manual testing was performed due to large portion of environment sensitive code and limited
time.

4. Usage
--------

To print movie labels: start application, enter movie Id and select printer from list,
then click "Print" button.
