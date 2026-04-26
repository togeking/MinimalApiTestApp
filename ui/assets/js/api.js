// 💡 バックエンドのURL（Program.csで起動しているURLに合わせてくれ）
const API_BASE_URL = 'http://localhost:5063/api';

// Axiosの共通設定（URLの土台をセットしておく）
const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// 全件取得
export const getTodos = async () => {
    try {
        const response = await apiClient.get('/Todos');
        return response.data; // Axiosなら .json() への変換が不要だ！
    } catch (error) {
        console.error('Todoの取得に失敗したぜ:', error);
        return [];
    }
};

// 新規作成
export const createTodo = async (todoData) => {
    try {
        const response = await apiClient.post('/Todos', todoData);
        return response.data;
    } catch (error) {
        console.error('Todoの作成に失敗したぜ:', error);
        throw error;
    }
};

// 更新
export const updateTodo = async (id, todoData) => {
    try {
        await apiClient.put(`/Todos/${id}`, todoData);
    } catch (error) {
        console.error('Todoの更新に失敗したぜ:', error);
        throw error;
    }
};

// 削除
export const deleteTodo = async (id) => {
    try {
        await apiClient.delete(`/Todos/${id}`);
    } catch (error) {
        console.error('Todoの削除に失敗したぜ:', error);
        throw error;
    }
};