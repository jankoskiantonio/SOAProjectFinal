'use strict';
import axios from 'axios';

// Full config:  https://github.com/axios/axios#request-config
// axios.defaults.baseURL = process.env.baseURL || process.env.apiUrl || '';
// axios.defaults.headers.common['Authorization'] = AUTH_TOKEN;
// axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';

const config = {
	baseURL: 'http://localhost:5088/api/',
	timeout: 60 * 1000, // Timeout
	withCredentials: false, // Check cross-site Access-Control
	Accept: '*/*',
};


const api = axios.create(config);
// api.interceptors.request.use((config) => {
// 	config.headers['Access-Control-Allow-Origin'] = '*';
// 	config.headers['Access-Control-Allow-Methods'] = 'GET, POST, PUT, DELETE';
// 	config.headers['Access-Control-Allow-Headers'] = 'Origin, X-Requested-With, Content-Type, Accept, Authorization';
// 	return config;
//   }, (error) => {
// 	return Promise.reject(error);
//   });

export function setAuthToken (token) {
	api.defaults.headers.Authorization = token ? `Bearer ${token}` : null;
}
export default api;
