import Repository from '../../repositories/RepositoryFactory';
const AuthRepository = Repository.get('auth');
import { setAuthToken } from '@/plugins/axios';

export default {
	namespaced: true,
	state: {
		token: null,
	},
	mutations: {
		GET_TOKEN (state, payload) {
			state.token = payload;
			console.log(payload);
			setAuthToken(payload.token);
		},
		
	},
	actions: {
		async login ({ commit }, data) {
			commit('GET_TOKEN', await AuthRepository.login(data));
		},
		
	},
	getters: {
		getToken (state) {
			return state.token;
		},
	},
};
