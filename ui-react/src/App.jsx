import { useState, useEffect } from 'react';
import { getTodos, createTodo, updateTodo, deleteTodo } from './api.js';
import './App.css';

function App() {
  // 💡 Reactの魔法「State（状態）」。この変数が変わると画面が勝手に再描画される！
  const [todos, setTodos] = useState([]);
  
  // フォーム入力値のState
  const [title, setTitle] = useState('');
  const [assigneeId, setAssigneeId] = useState(1);
  const [status, setStatus] = useState('NotStarted');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');

  // 💡 画面が最初に表示された時に1回だけ動く処理（一覧の取得）
  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    try {
      const data = await getTodos();
      setTodos(data); // データをStateにセット！これだけで一覧が画面に表示される
    } catch (error) {
      console.error('Todoの取得に失敗しました', error);
    }
  };

  // タスク追加ボタンが押された時の処理
  const handleSubmit = async (e) => {
    e.preventDefault();
    const newTodo = {
      title,
      userId: 1, // ※一旦ログインユーザーは1と仮定
      assigneeId: parseInt(assigneeId, 10),
      status,
      startDate: startDate || null,
      endDate: endDate || null
    };

    try {
      await createTodo(newTodo);
      // フォームの入力を空に戻す
      setTitle('');
      setAssigneeId(1);
      setStatus('NotStarted');
      setStartDate('');
      setEndDate('');
      // 一覧を再取得して画面を更新
      fetchTodos();
    } catch (error) {
      console.error('作成失敗', error);
      alert('作成に失敗しました');
    }
  };

  // ステータスが変更された時の処理
  const handleStatusChange = async (todo, newStatus) => {
    const updatedTodo = { ...todo, status: newStatus };
    try {
      await updateTodo(todo.id, updatedTodo);
      fetchTodos();
    } catch (error) {
      console.error('更新失敗', error);
    }
  };

  // 削除ボタンが押された時の処理
  const handleDelete = async (id) => {
    if (!window.confirm('本当に削除するかい？')) return;
    try {
      await deleteTodo(id);
      fetchTodos();
    } catch (error) {
      console.error('削除失敗', error);
    }
  };

  // 💡 ここから下がHTML（JSX）の定義。Stateの値がそのまま画面に反映される！
  return (
    <div className="container">
      <h1>🚀 My Todo App (React Edition)</h1>

      <div className="card">
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>タスク名</label>
            <input 
              type="text" 
              value={title} 
              onChange={(e) => setTitle(e.target.value)} 
              required 
              placeholder="例: Reactのコンポーネントを理解する" 
            />
          </div>
          <div className="form-row">
            <div className="form-group">
              <label>担当者ID</label>
              <input 
                type="number" 
                value={assigneeId} 
                onChange={(e) => setAssigneeId(e.target.value)} 
                required 
              />
            </div>
            <div className="form-group">
              <label>ステータス</label>
              <select value={status} onChange={(e) => setStatus(e.target.value)}>
                <option value="NotStarted">未着手</option>
                <option value="InProgress">進行中</option>
                <option value="Completed">完了</option>
              </select>
            </div>
          </div>
          <div className="form-row">
            <div className="form-group">
              <label>開始日</label>
              <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
            </div>
            <div className="form-group">
              <label>締切日</label>
              <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
            </div>
          </div>
          <button type="submit" className="btn-primary">タスクを追加</button>
        </form>
      </div>

      <div className="todo-list">
        {todos.length === 0 ? (
          <p>現在タスクはありません。遊ぼうぜ！</p>
        ) : (
          todos.map(todo => (
            <div key={todo.id} className={`todo-card ${todo.status === 'Completed' ? 'completed' : ''}`}>
              <div className="todo-content">
                <h3>{todo.title}</h3>
                <p className="meta">
                  担当者ID: {todo.assigneeId} | 締切: {todo.endDate || '未定'}
                </p>
              </div>
              <div className="todo-actions">
                <select 
                  value={todo.status} 
                  onChange={(e) => handleStatusChange(todo, e.target.value)}
                  className="status-select"
                >
                  <option value="NotStarted">未着手</option>
                  <option value="InProgress">進行中</option>
                  <option value="Completed">完了</option>
                </select>
                <button onClick={() => handleDelete(todo.id)} className="btn-danger">削除</button>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
}

export default App;