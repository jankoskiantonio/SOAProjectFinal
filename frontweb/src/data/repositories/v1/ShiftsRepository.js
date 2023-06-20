
import _axios from '@/plugins/axios';

export default {
	async getDepartment () {
		const response = await _axios.get('/Department');
		if (response.status === 200) {
			return response.data;
		}
	},
	async editShifts (body) {
		const response = await _axios.put(`/Department/${body.id}`, body);
		console.log(response.status);
		if (response.status === 200) {
			return response.data;
		}
	},
	async deletedShifts (id) {
		const response = await _axios.delete(`/Department/${id}`);
		console.log(response.status);
		if (response.status === 204) {
			return response.data;
		}
	},
	async addDepartment (body) {
		const response = await _axios.post('/Department', body);
		if (response.status === 200) {
			return response.data;
		}
	},
	async editDepartment (body) {
		const response = await _axios.put(`/Department/${body.departmentId	}`, body);
		if (response.status === 200) {
			return response.data;
		}
	},
	async deleteDepartment (body) {
		const response = await _axios.delete(`/Department/${body.departmentId}`);
		if (response.status === 200) {
			return response.data;
		}
	},
};
