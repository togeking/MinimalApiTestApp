import axios from 'axios';

const API_BASE_URL = 'http://localhost:5063/api';

const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

export const getTodos = async () => {
    const response = await apiClient.get('/Todos');
    return response.data;
};

export const createTodo = async (todoData) => {
    const response = await apiClient.post('/Todos', todoData);
    return response.data;
};

export const updateTodo = async (id, todoData) => {
    await apiClient.put(`/Todos/${id}`, todoData);
};

export const deleteTodo = async (id) => {
    await apiClient.delete(`/Todos/${id}`);
};