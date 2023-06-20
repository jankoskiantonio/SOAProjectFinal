<template>
  <v-app>
    <v-main>
      <LoginScreen v-if="!close" @closeLogin="closeScreen"/>
      <HomeScreen v-if="close"/>
    </v-main>
  </v-app>
</template>

<script>
import HomeScreen from './components/HomeScreen.vue';
import LoginScreen from './components/LoginScreen.vue'
export default {
  name: 'App',

  components: {
    HomeScreen,
    LoginScreen
  },

  data: () => ({
    close: false
  }),
  methods: {
    closeScreen(){
      this.close=true;
    }
  }
};
</script>
