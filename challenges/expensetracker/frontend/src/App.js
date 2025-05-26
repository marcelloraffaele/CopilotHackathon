import React, { useEffect, useState } from 'react';
import './App.css';
import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const API_BASE = 'http://localhost:5086/api/Expenses';

function App() {
  const [categories, setCategories] = useState([]);
  const [expenses, setExpenses] = useState([]);
  const [form, setForm] = useState({ amount: '', category: '', description: '', date: new Date().toISOString().slice(0, 10) });
  const [monthlyTotal, setMonthlyTotal] = useState(0);
  const [categorySummary, setCategorySummary] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch(`${API_BASE}/categories`).then(r => r.json()).then(setCategories);
    fetchExpenses();
    fetchMonthlyTotal();
    fetchCategorySummary();
  }, []);

  function fetchExpenses() {
    fetch(API_BASE)
      .then(r => r.json())
      .then(setExpenses);
  }

  function fetchMonthlyTotal() {
    fetch(`${API_BASE}/summary/monthly`)
      .then(r => r.json())
      .then(setMonthlyTotal);
  }

  function fetchCategorySummary() {
    fetch(`${API_BASE}/summary/category`)
      .then(r => r.json())
      .then(setCategorySummary)
      .finally(() => setLoading(false));
  }

  function handleChange(e) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  function handleSubmit(e) {
    e.preventDefault();
    const payload = { ...form, amount: parseFloat(form.amount), date: form.date };
    fetch(API_BASE, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    })
      .then(() => {
        setForm({ amount: '', category: '', description: '', date: new Date().toISOString().slice(0, 10) });
        fetchExpenses();
        fetchMonthlyTotal();
        fetchCategorySummary();
      });
  }

  function handleDelete(id) {
    fetch(`${API_BASE}/${id}`, { method: 'DELETE' })
      .then(() => {
        fetchExpenses();
        fetchMonthlyTotal();
        fetchCategorySummary();
      });
  }

  const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#A28CFF', '#FF6699', '#FF4444', '#44FF44', '#4444FF', '#AAAAAA'];

  return (
    <div className="App">
      <h1>Expense Tracker</h1>
      <form onSubmit={handleSubmit} className="expense-form">
        <input name="amount" type="number" step="0.01" placeholder="Amount" value={form.amount} onChange={handleChange} required />
        <select name="category" value={form.category} onChange={handleChange} required>
          <option value="">Category</option>
          {categories.map((cat, i) => <option key={i} value={cat}>{cat}</option>)}
        </select>
        <input name="description" type="text" placeholder="Description" value={form.description} onChange={handleChange} />
        <input name="date" type="date" value={form.date} onChange={handleChange} />
        <button type="submit">Add Expense</button>
      </form>

      <h2>Current Month Total: ${monthlyTotal.toFixed(2)}</h2>

      <h2>Spending by Category</h2>
      <div style={{ width: '100%', maxWidth: 400, margin: '0 auto' }}>
        {loading ? <p>Loading chart...</p> :
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie data={categorySummary} dataKey="total" nameKey="category" cx="50%" cy="50%" outerRadius={100} label>
                {categorySummary.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                ))}
              </Pie>
              <Tooltip />
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        }
      </div>

      <h2>All Expenses</h2>
      <table className="expense-table" style={{ margin: '0 auto' }}>
        <thead>
          <tr>
            <th>Date</th>
            <th>Amount</th>
            <th>Category</th>
            <th>Description</th>
            <th>Delete</th>
          </tr>
        </thead>
        <tbody>
          {expenses.map(exp => (
            <tr key={exp.id}>
              <td>{exp.date.slice(0, 10)}</td>
              <td>${exp.amount.toFixed(2)}</td>
              <td>{exp.category}</td>
              <td>{exp.description}</td>
              <td><button onClick={() => handleDelete(exp.id)}>Delete</button></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default App;
