import ShiftsRepository from './v1/ShiftsRepository';
import UsersRepository from	'./v1/UsersRepository';
import AuthRepository from './v1/AuthRepository';

const repositories = {
	shifts: ShiftsRepository,
	users: UsersRepository,
	auth: AuthRepository
};
export default {
	get: (name) => {
		if (repositories[name]) {
			return repositories[name];
		} else {
			throw new Error('Invalid Repository Type');
		}
	},
};
