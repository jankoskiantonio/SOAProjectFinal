import Repository from '../../repositories/RepositoryFactory';
const ShiftsRepository = Repository.get('shifts');

export default {
	namespaced: true,
	state: {
		shifts: null,
		editedShift: null,
		deletedShift: null,
	},
	mutations: {
		GET_SHIFTS (state, payload) {
			state.shifts = payload;
		},
		DELETED_SHIFT (state, payload) {
			state.deletedShift = payload;
		},
		EDITED_SHIFT (state, payload) {
			state.deletedShift = payload;
		},
		ADD_SHIFT (state, payload) {
			state.addShift = payload;
		},
		
	},
	actions: {
		async getDepartment ({ commit }, data) {
			commit('GET_SHIFTS', await ShiftsRepository.getDepartment(data));
		},
		async editShifts ({ commit }, body) {
			commit('EDITED_SHIFT', await ShiftsRepository.editShifts(body));
		},
		async deleteShifts ({ commit }, id) {
			commit('DELETED_SHIFT', await ShiftsRepository.deletedShifts(id));
		},
		async addDepartment ({ commit }, body) {
			commit('ADD_SHIFT', await ShiftsRepository.addDepartment(body));
		},
		async editDepartment ({ commit }, body) {
			commit('ADD_SHIFT', await ShiftsRepository.editDepartment(body));
		},
		async deleteDepartment ({ commit }, body) {
			commit('ADD_SHIFT', await ShiftsRepository.deleteDepartment(body));
		},
	},
	getters: {
		getShifts (state) {
			return state.shifts;
		},
	},
};
