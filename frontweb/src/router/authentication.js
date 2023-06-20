import Repository from '../data/repositories/RepositoryFactory';
import store from '../data/store';
// import { PERMISSIONS } from './permissions'

const AuthenticationRepository = Repository.get('authentication');

/**
 * Check if the user has a session.
 *
 * NOTE: How we do this will change when we migrate to APIv2.
 *
 * @param {boolean} checkExpiration Whether to test the API token agains the server
 */
export async function checkLogin (checkExpiration = false) {
	const getterName = 'authentication/hasStoredSession';
	if (store.getters[getterName]) {
		if (checkExpiration) {
			try {
				const serverResult = await AuthenticationRepository.checkAuth();

				if (!serverResult) {
					// If the server says the token is invalid, clear it from localStorage
					logout();
				}
				return serverResult;
			} catch (error) {
				// If there was a server error, assume logged out.
				return false;
			}
		} else {
			return true;
		}
	}
	return false;
}

var initialized = false;

/**
 * Vue-Router guard to check authentication
 *
 * @param {*} to
 * @param {*} from
 * @param {*} next
 */
export function requireAuth (to, from, next) {
	if (!initialized) {
		store.commit('authentication/initialize');
		initialized = true;
	}

	if (to.matched.some(record => record.meta.requireAuth)) {
		checkLogin(to.meta.requireAuth).then(loggedIn => {
			if (loggedIn) {
				next();
			} else {
				next({
					path: '/auth/login',
					query: { redirect: to.fullPath },
				});
			}
		});
	}
	next();
}

export async function logout () {
	store.dispatch('authentication/doLogout');
	store.commit('alerts/clear');
}
