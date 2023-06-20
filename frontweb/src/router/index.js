import Vue from 'vue';
import VueRouter from 'vue-router';
import { requireAuth } from '@/router/authentication.js';

Vue.use(VueRouter);

const routes = [
	{
		path: '/',
		meta: { requireAuth: true },
		component: () => import('@/views/Index'),
		children: [
			{
				path: '/',
				name: 'Dashboard',
				component: () => import('@/components/HomeScreen'),
			},
			{
				path: '/patientdetails/:id(\\d+)',
				name: 'Patient Details',
				component: () => import('@/views/Client/components/PatientDetails'),
			},
			{
				path: '/devicemeasurements/:id(\\d+)', // Expr. matches only numbers!',
				name: 'Device Measurements',
				component: () => import('@/views/Client/partials/DeviceMeasurements'),
			},
			{
				path: '/organizations',
				name: 'Organizations',
				component: () => import('@/views/Admin/MedicalInstitutions'),
			},
			{
				path: '/admins',
				name: 'Admins',
				component: () => import('@/views/Admin/Admins'),
			},
			{
				path: '/offices',
				name: 'Offices',
				component: () => import('@/views/Admin/Hospitals'),
			},
			{
				path: '/patients',
				name: 'Patients',
				component: () => import('@/views/Client/partials/Patients'),
			},
			{
				path: '/billings',
				name: 'Billings',
				component: () => import('@/views/Client/partials/Billings'),
			},
			{
				path: '/medicalteam',
				name: 'Medical Team',
				component: () => import('@/views/Client/partials/MedicalTeam'),
			},
			{
				path: '/medicalstaff',
				name: 'Medical Staff',
				component: () => import('@/views/Client/partials/MedicalStaff'),
			},
			{
				path: '/support',
				name: 'Support',
				component: () => import('@/views/Client/partials/Support'),
			},
			{
				path: '/Verifier',
				name: 'Verifier',
				component: () => import('@/views/Admin/Verifire'),
			},
			{
				path: '/mentalhealthtemplate',
				name: 'Mental Health Template',
				component: () => import('@/views/Client/partials/MentalHealth'),
			},
			{
				path: '/mentalhealthpatient',
				name: 'Mental Health Patient',
				component: () => import('@/views/Client/partials/MentalHealthPatients'),
			},
		],
	},
	{
		path: '/auth',
		redirect: '/auth/login',
		name: 'Login',
		component: () => import('@/views/authentication/Login'),
		children: [
			{
				path: '/auth/login',
				component: () => import('@/views/authentication/Login'),
			},
		],
	},
	{
		path: '/api/user/validatetoken',
		name: 'Confirm Validation',
		component: () => import('@/views/authentication/ResetPassword'),
	},
	{
		path: '/videoRoom/:id(\\d+)/:userId(\\d+)/:appointmentId(\\d+)',
		meta: { reqviewsreAuth: true },
		name: 'VideoRoom',
		component: () => import('@/views/Client/partials/VideoRoom'),
	},
	{
		path: '/auth/forgetpassword',
		name: 'Forget Password',
		component: () => import('@/views/authentication/ForgetPassword'),
	},
	{
		path: '/measurements',
		name: 'HTML Rendering',
		component: () => import('@/views/Client/partials/no-auth/RenderData'),
	},
	{
		path: '/measurements/:hubIdentifier',
		name: 'Patient Report',
		component: () => import('@/views/Client/partials/no-auth/RenderDataForPatient'),
	},
	{
		path: '/:pathMatch(.*)*', redirect: '/',
	},
];
const router = new VueRouter({
	mode: 'history',
	// base: process.env.BASE_URL,
	routes: routes,
});

router.beforeEach(requireAuth);
export default router;
