# Solution

Split the solution into backend and frontend components, focusing on a simple expense tracker application that allows users to log expenses, view summaries, and visualize spending by category.

# Backend
In agent mode run the following prompt:
```markdown
Work in the directory backend. When run terminal remember to change the right directory, for example: `cd challenges/expensetracker/backend && <command>`.
Build an **Expense Tracker** microservice that helps users log daily expenses and visualize their spending patterns. The system should allow users to add expenses with categories, view spending summaries, and get insights into their financial habits.

Initialize a new dotnet web API project in the backend directory.

The backend should be a RESTful API should be built using dotnet core and implement the following user stories:
1. As a user, I can add a new expense with amount, category, and description
2. As a user, I can view all my expenses in a list format
3. As a user, I can see total spending for the current month
4. As a user, I can view spending breakdown by category
5. As a user, I can delete expenses I added by mistake
The expense object should have the following fields: id, amount, category, date, description
The backend should use an in-memory array/list (no database needed).
Use a fixed list of 10 categories like ["Food", "Transport", "Entertainment", "Shopping", "Bills"] please add the remaining.
When insert a new expense, use current date as default, allow manual date selection.
Enable CORS for all origins.
Be sure that the controller is configured in the `program.cs`.
Update the file `.http` add all the api for test, add also the `<host>:<port>/openapi/v1.json`.
```

Download the api description file from the `<host>:<port>/openapi/v1.json` and save it as `openapi.json`.

# Frontend
In agent mode run, add the openapi.json file as attachment and che frontend folder and the following prompt:
```markdown
Work in the directory frontend. When run terminal remember to change the right directory, for example: `cd challenges/expensetracker/frontend && <command>`.

Build an **Expense Tracker** frontend that helps users log daily expenses and visualize their spending patterns. The system should allow users to add expenses with categories, view spending summaries, and get insights into their financial habits.
Initialize a new react project in the frontend directory
The frontend should consume the RESTful API provided by the backend and implement the following user stories:
1. As a user, I can add a new expense with amount, category, and description
2. As a user, I can view all my expenses in a list format
3. As a user, I can see total spending for the current month
4. As a user, I can view spending breakdown by category
5. As a user, I can delete expenses I added by mistake
When insert a new expense, use current date as default, allow manual date selection.
Implement a clear view of current month spending and a simple chart showing spending by category.
Use Chart.js, Recharts, or simple CSS bars for visualization.
```



### Key Features:
1. **Quick Entry**: Fast expense logging with minimal fields
2. **Visual Feedback**: Immediate updates to totals and charts
3. **Category Organization**: Logical grouping of expenses
4. **Monthly Focus**: Clear view of current month spending
5. **Delete Protection**: Easy removal of incorrect entries

```