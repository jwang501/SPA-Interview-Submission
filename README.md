#Submission for the first-round interview of the Software Engineer position with Sponsored Programs Administration.

Assumptions made regarding ambiguous requirements:
1. All spreadsheets have at least one "Period" column and not just a "Total" column.
2. For each subrecipient row in the spreadsheet, there is an associated "Exempt Subaward Costs" row with is also included in the total subaward amount.
3. For subrecipient rows missing a subrecipient name are not printed; an error is printed instead.
4. For duplicate subrecipient rows, the sum of the amounts is returned.
5. For spreadsheets where the total cost is split into "Sponsor" and "Cost Share" columns, the total subaward amount is the sum of the two columns.
6. 

Questions to ask:
1. The exercise states that "any spreadsheet in this format" should be processed. Is this referring to how the cells are arranged in the spreadsheet, or the file format?
2. Are there any other small variations in formatting that the spreadsheets may have? (Subaward: name vs Subaward - name, uppercase vs lowercase)
3. How should we treat duplicate rows? (Add amounts together or exclude duplicates?)
4. Should we support reading a single file?
