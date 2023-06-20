
import _axios from '@/plugins/axios';

export default {
	async login (data) {
		const response = await _axios.post('/Auth/login', data);
		console.log(response);
		if (response.status === 200) {
			return response.data;
		}
	},
};
