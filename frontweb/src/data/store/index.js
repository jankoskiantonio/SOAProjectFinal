import Vue from 'vue';
import Vuex from 'vuex';
import modules from './modules';

Vue.use(Vuex);

export default new Vuex.Store({
	state: {
		drawer: true,
		pageSize: { height: 500, width: 500 },
	},
	getters: {
		pageHeight: (state) => (add = 0) => { return add + state.pageSize.height + 'px'; },
	},
	mutations: {
		SET_DRAWER (state, payload) {
			state.drawer = payload;
		},
		SET_SCREEN_SIZE (state, payload) {
			state.pageSize = payload.height > 555 ? { height: payload.height - 350, width: payload.width } : { height: 200, width: payload.width };
		},
	},
	actions: {
		async setupWebSockets (store) {
			await store.dispatch('socket/setConnection');
		},
	},
	modules,
});
