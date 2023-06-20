import Repository from '../../repositories/RepositoryFactory';
const UsersRepository = Repository.get('users');

export default {
	namespaced: true,
	state: {
		users: null,
		usersDetails: null,
		usersPayments: null,
		job: null,
		manager: null,
		employee: null,
		newjob: null,
		addmanager:null,
	},
	mutations: {
		GET_USERS (state, payload) {
			state.users = payload;
		},
		GET_USERS_DETAILS (state, payload) {
			state.usersDetails = payload;
		},
		GET_USERS_PAYMENTS (state, payload) {
			state.usersPayments = payload;
		},
		GET_JOB (state, payload) {
			state.job = payload;
		},
		GET_MANAGER (state, payload) {
			state.manager = payload;
		},
		ADD_EMPLOYEE (state, payload) {
			state.employee = payload;
		},
		ADD_JOB (state, payload) {
			state.newjob = payload;
		},
		ADD_MANAGER (state, payload) {
			state.addmanager = payload;
		}
	},
	actions: {
		async getUsers ({ commit }, data) {
			commit('GET_USERS', await UsersRepository.getUsers(data));
		},
		async getUsersDetails ({ commit }, name) {
			commit('GET_USERS_DETAILS', await UsersRepository.getUsersDetails(name));
		},
		async getUsersPayments ({ commit }, name) {
			commit('GET_USERS_PAYMENTS', await UsersRepository.getUsersPayments(name));
		},
		async getJobs ({ commit }) {
			commit('GET_JOB', await UsersRepository.getJobs());
		},
		async getManager ({ commit }) {
			commit('GET_MANAGER', await UsersRepository.getManager());
		},
		async addEmployee({ commit }, body) {
			commit('ADD_EMPLOYEE', await UsersRepository.addEmployee(body));
		},
		async addJob({ commit }, body) {
			commit('ADD_JOB', await UsersRepository.addJob(body));
		},
		async addManager({ commit }, body) {
			commit('ADD_MANAGER', await UsersRepository.addManager(body));
		},
		async editEmployee({ commit }, body) {
			commit('ADD_EMPLOYEE', await UsersRepository.editEmployee(body));
		},
		async editJob({ commit }, body) {
			commit('ADD_JOB', await UsersRepository.editJob(body));
		},
		async editManager({ commit }, body) {
			commit('ADD_MANAGER', await UsersRepository.editManager(body));
		},
		async deleteEmployee({ commit }, body) {
			commit('ADD_MANAGER', await UsersRepository.deleteEmployee(body));
		},
		async deleteJob({ commit }, body) {
			commit('ADD_MANAGER', await UsersRepository.deleteJob(body));
		},
		async deleteManager({ commit }, body) {
			commit('ADD_MANAGER', await UsersRepository.deleteManager(body));
		}
	},
	getters: {
		getUsers (state) {
			return state.users;
		},
		getUsersDetails (state) {
			return state.usersDetails;
		},
		getUsersPayments (state) {
			return state.usersPayments;
		},
		getJobs (state) {
			return state.job;
		},
		getManager (state) {
			return state.manager;
		},
	},
};
