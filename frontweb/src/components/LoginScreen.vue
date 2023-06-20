<template>
<div class="d-flex align-center justify-center">
 <v-card  width="70%" height="60%">
  <v-row class="ma-5">
     <v-col
          cols="12"
          sm="6"
        >
          <v-text-field
            name="input-10-2"
            label="Email"
            v-model="email"
            value="wqfasds"
            class="input-group--focused"
          ></v-text-field>
        </v-col>
        
        <v-col
          cols="12"
          sm="6"
        >
          <v-text-field
            v-model="password"
            :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
            :rules="[rules.required, rules.min]"
            :type="show1 ? 'text' : 'password'"
            name="input-10-1"
            label="Password"
            hint="At least 8 characters"
            counter
            @click:append="show1 = !show1"
          ></v-text-field>
        </v-col>
  </v-row>
        <v-btn @click="login" class="mx-5 mb-5">Login</v-btn>
        <v-btn class="mx-5 mb-5">Register</v-btn>
        <v-btn @click="test" class="mx-5 mb-5">Test</v-btn>
 </v-card>
 </div>
</template>

<script>
import { mapGetters } from 'vuex';
// import { mapGetters } from 'vuex';
  export default {
    data () {
      return {
        email: '',
        show1: false,
        show2: true,
        show3: false,
        show4: false,
        password: '',
        rules: {
          required: value => !!value || 'Required.',
          min: v => v.length >= 8 || 'Min 8 characters',
          emailMatch: () => (`The email and password you entered don't match`),
        },
      }
    },
    computed: {
      ...mapGetters({ 
        token: 'auth/getToken', 
      }),
      tokenFromRepo(){
        return this.token;
      }
    },
    methods: {
      async login(){
          var body = {
            'email': this.email,
            'password': this.password
          }
        await this.$store.dispatch('auth/login', body);
        this.$emit('closeLogin');
      },
      test (){
        this.$store.dispatch('shifts/getShifts')
      }
    }
  }
</script>