import Vue from 'vue';
import App from './App.vue';
import vuetify from './plugins/vuetify';
// import axios from 'axios';
import store from './data/store';
import './plugins/axios';

// Vue.config.productionTip = false;
// console.log(process.env.VUE_APP_API_URL);
// const axiosInstance = axios.create({
//   baseURL: process.env.VUE_APP_API_URL,
//   timeout: 60 * 1000,
//   withCredentials: false,
// });

// Vue.prototype.$http = axiosInstance; // Set axios as a global property

new Vue({
  vuetify,
  store,
  render: h => h(App),
}).$mount('#app');
