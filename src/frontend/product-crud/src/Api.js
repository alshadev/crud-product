import axios from 'axios';

const apiUrl = 'http://localhost:7123/api/v1/Product';
const apiIdentityUserUrl = 'http://localhost:7456/api/v1/User'

const setAuthHeader = (token) => {
    return {
        headers: {
            'Authorization': `Bearer ${token}`,
        }
    };
};

export const login = async (username, password) => {
    try {
        const response = await axios.post(`${apiIdentityUserUrl}/Login`, { username, password });
        
        if (response.data) {
            console.log(response.data);
            localStorage.setItem('authToken', response.data);
            return response.data;
        } else {
            throw new Error('Login failed: No token received.');
        }
    } catch (error) {
        throw new Error('Invalid credentials or server error');
    }
};

export const register = async (username, password) => {
    try {
        const response = await axios.post(`${apiIdentityUserUrl}/Register`, { username, password });
        return response.data;
    } catch (error) {
        throw new Error('Registration failed');
    }
};

export const getProducts = () => {
    const token = localStorage.getItem('authToken');
    if (!token) {
        throw new Error('No token found, please log in first.');
    }

    return axios.get(`${apiUrl}`, setAuthHeader(token));
};

export const getProductById = (id) => {
    const token = localStorage.getItem('authToken');
    if (!token) {
        throw new Error('No token found, please log in first.');
    }

    return axios.get(`${apiUrl}/${id}`, setAuthHeader(token));
};

export const createProduct = (productData) => {
    const token = localStorage.getItem('authToken');
    if (!token) {
        throw new Error('No token found, please log in first.');
    }

    return axios.post(`${apiUrl}`, productData, setAuthHeader(token));
};

export const updateProduct = (productData) => {
    const token = localStorage.getItem('authToken');
    if (!token) {
        throw new Error('No token found, please log in first.');
    }

    return axios.put(`${apiUrl}`, productData, setAuthHeader(token));
};

export const deleteProduct = (id) => {
    const token = localStorage.getItem('authToken');
    if (!token) {
        throw new Error('No token found, please log in first.');
    }

    return axios.delete(`${apiUrl}/${id}`, setAuthHeader(token));
};