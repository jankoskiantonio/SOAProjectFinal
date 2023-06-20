
import _axios from '@/plugins/axios';

export default {
	async getUsers () {
		const response = await _axios.get('/Employee');
		if (response.status === 200) {
			return response.data;
		}
	},
	async getUsersDetails (name) {
		const response = await _axios.get(`/showEmployee/${name}`);
		if (response.status === 200) {
			console.log(response.data.data);
			return response.data.data;
		}
	},
	async getUsersPayments (name) {
		const response = await _axios.get(`/payments/${name}`);
		if (response.status === 200) {
			console.log(response.data.data);
			return response.data.data;
		}
	},
	async getJobs () {
		const response = await _axios.get(`/Job`);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async addJob(obj) {
		const response = await _axios.post(`/Job`, obj);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async getManager () {
		const response = await _axios.get(`/Manager`);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async addEmployee (data) {
		const response = await _axios.post(`/Employee`, data);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async addManager (data) {
		const response = await _axios.post(`/Manager`, data);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async editEmployee (data) {
		const response = await _axios.put(`/Employee/${data.employeeId}`, data);
		if (response.status === 200) {
			return response.data;
		}
	},
	async editJob (data) {
		const response = await _axios.put(`/Job/${data.jobId}`, data);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async editManager (data) {
		const response = await _axios.put(`/Manager/${data.managerId}`, data);
		if (response.status === 200) {
			
			return response.data;
		}
	},

	async deleteEmployee (data) {
		const response = await _axios.delete(`/Employee/${data.employeeId}`);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async deleteJob (data) {
		const response = await _axios.delete(`/Manager/${data.jobId}`);
		if (response.status === 200) {
			
			return response.data;
		}
	},
	async deleteManager (data) {
		const response = await _axios.delete(`/Manager/${data.managerId}`);
		if (response.status === 200) {
			
			return response.data;
		}
	},
};
