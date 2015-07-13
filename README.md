# sort-names

## Programming Exercise for GlobalX ##

**Requirements**

Write .NET console app that;

- Takes as a parameter a string that represents a text file containing a list of names.
- Orders the names by last name followed by first name.
- Creates a new text file called <input-file-name>-sorted.txt with the list of sorted names.

For example, if the input file contains:

    BAKER, THEODORE
    SMITH, ANDREW
    KENT, MADISON
    SMITH, FREDRICK

Then the output file would be:

    BAKER, THEODORE
    KENT, MADISON
    SMITH, ANDREW
    SMITH, FREDRICK

Example of console execution:

    sort-names c:\names.txt
    BAKER, THEODORE
    KENT, MADISON
    SMITH, ANDREW
    SMITH, FREDRICK
    Finished: created names-sorted.txt

**Notes**

- Solution follows example fully.
- Unit tests exist, with NUnit Test Adapter for VS2012, 2013 and 2015.
- The solution is in github (public repository) at [https://github.com/lowdavid/sort-names](https://github.com/lowdavid/sort-names).
- Tests are run automatically on checkin via a CI service at [https://ci.appveyor.com/project/lowdavid/sort-names](https://ci.appveyor.com/project/lowdavid/sort-names)