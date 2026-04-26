// api.jsから通信用の関数を輸入（インポート）する
import { getTodos, createTodo, updateTodo, deleteTodo } from './api.js';

// 画面の要素を取得
const todoForm = document.getElementById('todo-form');
const todoList = document.getElementById('todo-list');

// 💡 画面が読み込まれたら、タスク一覧を表示する
document.addEventListener('DOMContentLoaded', async () => {
    await renderTodos();
});

// タスク追加フォームが送信（Submit）された時の処理
todoForm.addEventListener('submit', async (e) => {
    e.preventDefault(); // 画面がリロードされるのを防ぐおまじないだ

    // フォームの入力値をかき集める
    const newTodo = {
        title: document.getElementById('title').value,
        userId: 1, // 今回は一旦「ログイン中のユーザーIDは1」と仮定するぜ
        assigneeId: parseInt(document.getElementById('assigneeId').value),
        status: document.getElementById('status').value,
        startDate: document.getElementById('startDate').value || null,
        endDate: document.getElementById('endDate').value || null
    };

    try {
        await createTodo(newTodo); // api.js に通信を依頼
        todoForm.reset(); // フォームを空にする
        await renderTodos(); // 画面を再描画
    } catch (error) {
        alert('タスクの追加に失敗しました。');
    }
});

// タスク一覧を描画する関数
const renderTodos = async () => {
    const todos = await getTodos(); // api.js からデータを取得
    todoList.innerHTML = ''; // 一旦リストを空にする

    if (todos.length === 0) {
        todoList.innerHTML = '<p>現在タスクはありません。遊ぼうぜ！</p>';
        return;
    }

    // 取得したタスクの数だけHTMLのカードを作る
    todos.forEach(todo => {
        const card = document.createElement('div');
        card.className = `todo-card ${todo.status === 'Completed' ? 'completed' : ''}`;
        
        // カードの中身のHTML
        card.innerHTML = `
            <div class="todo-content">
                <h3>${todo.title}</h3>
                <p class="meta">
                    担当者ID: ${todo.assigneeId} | 
                    締切: ${todo.endDate ? todo.endDate : '未定'}
                </p>
            </div>
            <div class="todo-actions">
                <select class="status-select" data-id="${todo.id}">
                    <option value="NotStarted" ${todo.status === 'NotStarted' ? 'selected' : ''}>未着手</option>
                    <option value="InProgress" ${todo.status === 'InProgress' ? 'selected' : ''}>進行中</option>
                    <option value="Completed" ${todo.status === 'Completed' ? 'selected' : ''}>完了</option>
                </select>
                <button class="btn-danger delete-btn" data-id="${todo.id}">削除</button>
            </div>
        `;

        // 💡 ステータスが変更された時のイベントを仕込む
        const statusSelect = card.querySelector('.status-select');
        statusSelect.addEventListener('change', async (e) => {
            const newStatus = e.target.value;
            await handleUpdateStatus(todo, newStatus);
        });

        // 💡 削除ボタンが押された時のイベントを仕込む
        const deleteBtn = card.querySelector('.delete-btn');
        deleteBtn.addEventListener('click', async () => {
            if (confirm('本当に削除するかい？')) {
                await deleteTodo(todo.id);
                await renderTodos(); // 再描画
            }
        });

        todoList.appendChild(card);
    });
};

// ステータス更新処理
const handleUpdateStatus = async (todo, newStatus) => {
    const updatedTodo = {
        id: todo.id,
        title: todo.title,
        userId: todo.userId,
        assigneeId: todo.assigneeId,
        startDate: todo.startDate,
        endDate: todo.endDate,
        status: newStatus
    };

    try {
        await updateTodo(todo.id, updatedTodo);
        await renderTodos(); // 再描画して、完了の取り消し線などを反映させる
    } catch (error) {
        alert('ステータスの更新に失敗しました。');
    }
};