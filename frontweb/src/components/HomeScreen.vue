<template>
  <v-card class="ma-5">
<h2>Department</h2>
  <v-data-table
  :loading="loader"
    :headers="headers"
    dense
    :items="getShiftFromRepo"
    class="elevation-1 my-5"
    :height="500"
    :search="search"
  >
  <!--  :items-per-page="-1" -->
    <template v-slot:top>
      <v-toolbar
        flat
      >
        <v-text-field
        append-icon="mdi-magnify"
          v-model="search"
          label="Search"
          class="mx-4"
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-text-field
        append-icon="mdi-filter"
          v-model="filterTotalPay"
          label="Filter"
          type="number"
          class="mx-4"
        ></v-text-field>
        <v-dialog
          v-model="dialogDepartment"
          max-width="500px"
        >
        <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="primary"
              dark
              class="mb-2"
              v-bind="attrs"
              v-on="on"
            >
              Create
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">Department</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                   <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-calendar"
                      v-model="editedItem.name"
                      label="fullName"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-account"
                      v-model="editedItem.location"
                      label="email"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-store"
                      v-model="editedItem.city"
                      label="phone"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="close"
              >
                Cancel
              </v-btn>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="save"
              >
                Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to delete this shift?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
              <v-btn color="blue-darken-1" variant="text" @click="deleteItemConfirm">OK</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item="{ item }">
      <tr>  
        <td>
          {{item.departmentId}}
        </td>  
        <td>
          {{item.name}}
        </td>  
        <td>
          {{item.location}}
        </td>  
        <td>
          {{item.city}}
        </td>  
        <td>
          <v-icon
        class="mr-2"
        @click="editItem(item, 'department')"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        @click="deleteItem(item, 'department')"
      >
        mdi-delete
      </v-icon>
        </td>  
      </tr>
    </template>
  </v-data-table>
  <h2>Emplyee</h2>
  <v-data-table
  :loading="loader"
    :headers="headerEmplyee"
    dense
    :items="getUsersFromRepo"
    class="elevation-1 my-5"
    :height="500"
    :search="search"
  >
  <!--  :items-per-page="-1" -->
    <template v-slot:top>
      <v-toolbar
        flat
      >
        <v-text-field
        append-icon="mdi-magnify"
          v-model="search"
          label="Search"
          class="mx-4"
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-text-field
        append-icon="mdi-filter"
          v-model="filterTotalPay"
          label="Filter"
          type="number"
          class="mx-4"
        ></v-text-field>
        <v-dialog
          v-model="dialogEmplyee"
          max-width="500px"
        >
        <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="primary"
              dark
              class="mb-2"
              v-bind="attrs"
              v-on="on"
            >
              Create
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-calendar"
                      v-model="editedItem.fullname"
                      label="fullName"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-account"
                      v-model="editedItem.email"
                      label="email"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-store"
                      v-model="editedItem.phone"
                      label="phone"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-clock"
                      v-model="editedItem.address"
                      label="address"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-cash"
                      v-model="editedItem.salary"
                      type="number"
                      label="salary"
                    ></v-text-field>
                  </v-col>
                   <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-cash"
                     type="number"
                      v-model="editedItem.dailyHours"
                      label="dailyHours"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-cash"
                     type="number"
                      v-model="editedItem.leaveDays"
                      label="leaveDays"
                    ></v-text-field>
                  </v-col>

                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-select
                    append-icon="mdi-chart-line"
                    :items="getShiftFromRepo"
                    item-text="name"
                    item-value="departmentId"
                    :value="editedItem.departmentId"
                      v-model="editedItem.departmentId"
                      label="departmentId"
                    ></v-select>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-select
                    append-icon="mdi-white-balance-sunny"
                    :items="getJobFromRepo"
                    item-text="jobName"
                    item-value="jobId"
                    :value="editedItem.jobId"
                      v-model="editedItem.jobId"
                      label="jobId"
                    ></v-select>
                  </v-col>
                  
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="close"
              >
                Cancel
              </v-btn>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="save"
              >
                Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to delete this shift?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
              <v-btn color="blue-darken-1" variant="text" @click="deleteItemConfirm">OK</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item="{ item }">
      <tr>  
        <td>
          {{item.fullName}}
        </td>  
        <td>
          {{item.email}}
        </td>  
        <td>
          {{item.address}}
        </td>  
        <td>
          {{item.dailyHours}}
        </td>  
        <td>
          {{item.phone}}
        </td>  
        <td>
          {{item.salary}}
        </td>  
        <td>
          {{item.leaveDays}}
        </td>  
        <td>
          {{item.hireDate}}
        </td>   
        <td>
          <v-icon
        class="mr-2"
        @click="editItem(item, 'employee')"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        @click="deleteItem(item, 'employee')"
      >
        mdi-delete
      </v-icon>
        </td>  
      </tr>
    </template>
  </v-data-table>
  <h2>Job</h2>
  <v-data-table
  :loading="loader"
    :headers="headersJob"
    dense
    :items="getJobFromRepo"
    class="elevation-1 my-5"
    :height="500"
    :search="search"
  >
  <!--  :items-per-page="-1" -->
    <template v-slot:top>
      <v-toolbar
        flat
      >
        <v-toolbar-title>2.3 and 2.4</v-toolbar-title>
        <v-divider
          class="mx-4"
          inset
          vertical
        ></v-divider>
        <v-text-field
        append-icon="mdi-magnify"
          v-model="search"
          label="Search"
          class="mx-4"
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-text-field
        append-icon="mdi-filter"
          v-model="filterTotalPay"
          label="Filter"
          type="number"
          class="mx-4"
        ></v-text-field>
        <v-dialog
          v-model="dialogJob"
          max-width="500px"
        >
        <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="primary"
              dark
              class="mb-2"
              v-bind="attrs"
              v-on="on"
            >
              Create
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-calendar"
                      v-model="editedItem.jobName"
                    ></v-text-field>
                  </v-col>
                  
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="close"
              >
                Cancel
              </v-btn>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="save"
              >
                Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to delete this shift?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
              <v-btn color="blue-darken-1" variant="text" @click="deleteItemConfirm">OK</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item="{ item }">
      <tr>  
        <td>
          {{item.jobId}}
        </td>  
        <td>
          {{item.jobName}}
        </td>
        <td>
          <v-icon
        class="mr-2"
        @click="editItem(item, 'job')"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        @click="deleteItem(item, 'job')"
      >
        mdi-delete
      </v-icon>
        </td>  
      </tr>
    </template>
  </v-data-table>
  <h2>Manager</h2>
  <v-data-table
  :loading="loader"
    :headers="headerEmplyee"
    dense
    :items="getManagerFromRepo"
    class="elevation-1 my-5"
    :height="500"
    :search="search"
  >
  <!--  :items-per-page="-1" -->
    <template v-slot:top>
      <v-toolbar
        flat
      >
        <v-toolbar-title>2.3 and 2.4</v-toolbar-title>
        <v-divider
          class="mx-4"
          inset
          vertical
        ></v-divider>
        <v-text-field
        append-icon="mdi-magnify"
          v-model="search"
          label="Search"
          class="mx-4"
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-text-field
        append-icon="mdi-filter"
          v-model="filterTotalPay"
          label="Filter"
          type="number"
          class="mx-4"
        ></v-text-field>
        <v-dialog
          v-model="dialog"
          max-width="500px"
        >
        <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="primary"
              dark
              class="mb-2"
              v-bind="attrs"
              v-on="on"
            >
              Create
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

           <v-card-text>
              <v-container>
                <v-row>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-calendar"
                      v-model="editedItem.fullname"
                      label="fullName"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-account"
                      v-model="editedItem.email"
                      label="email"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-store"
                      v-model="editedItem.phone"
                      label="phone"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-clock"
                      v-model="editedItem.address"
                      label="address"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-cash"
                      v-model="editedItem.salary"
                      type="number"
                      label="salary"
                    ></v-text-field>
                  </v-col>
                   <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-cash"
                     type="number"
                      v-model="editedItem.dailyHours"
                      label="dailyHours"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                    append-icon="mdi-cash"
                     type="number"
                      v-model="editedItem.leaveDays"
                      label="leaveDays"
                    ></v-text-field>
                  </v-col>

                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-select
                    append-icon="mdi-chart-line"
                    :items="getShiftFromRepo"
                    item-text="name"
                    item-value="departmentId"
                    :value="editedItem.departmentId"
                      v-model="editedItem.departmentId"
                      label="departmentId"
                    ></v-select>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-select
                    append-icon="mdi-white-balance-sunny"
                    :items="getJobFromRepo"
                    item-text="jobName"
                    item-value="jobId"
                    :value="editedItem.jobId"
                      v-model="editedItem.jobId"
                      label="jobId"
                    ></v-select>
                  </v-col>
                  
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="close"
              >
                Cancel
              </v-btn>
              <v-btn
                color="blue-darken-1"
                variant="text"
                @click="save"
              >
                Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to delete this shift?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
              <v-btn color="blue-darken-1" variant="text" @click="deleteItemConfirm">OK</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item="{ item }">
      <tr>  
        <td>
          {{item.fullName}}
        </td>  
        <td>
          {{item.email}}
        </td>  
        <td>
          {{item.address}}
        </td>  
        <td>
          {{item.dailyHours}}
        </td>  
        <td>
          {{item.phone}}
        </td>  
        <td>
          {{item.salary}}
        </td>  
        <td>
          {{item.leaveDays}}
        </td>  
        <td>
          {{item.hireDate}}
        </td>   
        <td>
          <v-icon
        class="mr-2"
        @click="editItem(item, 'manager')"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        @click="deleteItem(item, 'manager')"
      >
        mdi-delete
      </v-icon>
        </td>  
      </tr>
    </template>
  </v-data-table>
</v-card>
</template>

<script>
import axios from 'axios';
import { mapGetters } from 'vuex';
  export default {
    data: () => ({
      dialogDepartment: false,
      dialogEmplyee: false,
      dialogJob: false,
      loader: false,
      expanded: [],
      filteredData:[],
      searchEmployee: '',
      singleExpand: true,
      dialogDelete: false,
      filterTotalPay: null,
      headers: [
        { text: 'Id', value: 'departmentId' },
        { text: 'Name', value: 'name' },
        { text: 'Location', value: 'location' },
        { text: 'City', value: 'city' },
        { text: 'Edit/Delete' },
      ],
      headerEmplyee: [
        { text: 'Full Name', value: 'fullName' },
        { text: 'Email', value: 'email' },
        { text: 'Address', value: 'address' },
        { text: 'Daily Hours', value: 'dailyHours' },
        { text: 'Phone', value: 'phone' },
        { text: 'Salary', value: 'salary' },
        { text: 'Leave Days', value: 'leaveDays' },
        { text: 'Hire Date', value: 'hireDate' },
        { text: '', value: 'total_pay' },
      ],
      headersJob: [
        { text: 'Id', value: 'jobId' },
        { text: 'Name', value: 'jobName' },
        
        { text: '', value: 'total_pay' },
      ],
      managerHeader:[
      { text: 'Full Name', value: 'full_name' },
      { text: '', sortable: false },
      ],
      itemsStatus: ['Processing', 'Pending', 'Complete', 'Failed'],
      itemsShiftType: ['Day', 'Night', 'Holiday'],
      desserts: [],
      selectedFile: null,
      editedIndex: -1,
      search:'',
      dialog: false,
      editedItem: {
        fullname: null,
        jobName:null,
        email: null,
        phone: null,
        address: null,
        hairDate: null,
        salary: null,
        dailyHours: null,
        leaveDays: null,
        departmentId: null,
        jobId: null,
      },
      defaultItem: {
        jobName:null,
         fullname:null,
        email: null,
        phone: null,
        address: null,
        hairDate: null,
        salary: null,
        dailyHours: null,
        leaveDays: null,
        departmentId: null,
        jobId: null,
      },
    }),
    computed: {
		...mapGetters({ 
      shifts: 'shifts/getShifts', 
      employees: 'users/getUsers',
      job: 'users/getJobs',
      manager: 'users/getManager',
      userDetails: 'users/getUsersDetails',
      userPayments: 'users/getUsersPayments',
    }),
    getJobFromRepo(){
      return this.job ?? [];
    },
    getManagerFromRepo(){
      return this.manager ?? [];
    },
    getUserDetailsFromRepo(){
      return this.userDetails ?? ''
    },
    getShiftFromRepo(){
      return this.shifts ?? [];
    },
    getUsersFromRepo(){
      return this.employees ?? [];
    },
    getUsersPayments(){
      return this.userPayments?? [];
    },
      formTitle () {
        return this.editedIndex === -1 ? 'New Shift' : 'Edit Shift'
      },
    },
    watch: {
      filterTotalPay(val){
        this.filteredData = this.shifts.filter(item => item.total_pay > val);
      },
      expanded(val){
        if(val.length > 0){
          // this.$store.dispatch('users/getUsersDetails', val[0].full_name);
          // this.$store.dispatch('users/getUsersPayments', val[0].full_name);
        }
      },
      dialogDepartment (val) {
        val || this.close()
      },
      dialogEmplyee (val) {
        val || this.close()
      },
      dialogJob(val) {
        val || this.close()
      },
      dialogDelete (val) {
        val || this.closeDelete()
      },
    },
    created () {
      this.initialize()
    },
    methods: {
      async initialize () {
        this.loader = true;
        await this.$store
				.dispatch('shifts/getDepartment');
        await this.$store
				.dispatch('users/getUsers');
        this.loader = false;
        await this.$store
				.dispatch('users/getJobs');
        this.loader = false;
        await this.$store
				.dispatch('users/getManager');
        this.loader = false;
      },
      handleFileUpload(file) {
        this.selectedFile = file;
      },
      
      uploadFile() {
        this.loader = true;
        if (this.selectedFile) {
          const formData = new FormData();
          formData.append('csv_file', this.selectedFile);
          axios.post('http://127.0.0.1:8000/api/upload', formData, {
            headers: {
              'Content-Type': 'multipart/form-data'
            }
          })
            .then(() => {
              this.initialize()
            })
        }
      },
      editItem (item, dialog) {
        this.editedIndex = 0
        this.editedItem = Object.assign({}, item)
        if(dialog === 'department'){
           this.dialogDepartment = true;
        } else if(dialog === 'employee'){
           this.dialogEmplyee = true
        } else if(dialog === 'job'){
           this.dialogJob = true;
        } else if(dialog === 'manager'){   
          this.dialog = true;
        }
      },
      deleteItem (item, dialog) {
         if(dialog === 'department'){
           this.$store.dispatch('shifts/deleteDepartment', item)
           this.initialize()
        } else if(dialog === 'employee'){
           this.$store.dispatch('users/deleteEmplyee', item)
        } else if(dialog === 'job'){
          this.$store.dispatch('users/deleteJob', item)
        } else if(dialog === 'manager'){   
          this.$store.dispatch('users/deleteManager', item)
        }
      },
      deleteItemConfirm () {
        var body = Object.assign(this.shifts[this.editedIndex], this.editedItem).id;
        this.$store.dispatch('shifts/deleteShifts', body).then(() => {
          this.shifts.splice(this.editedIndex, 1)
          this.closeDelete()
        });
      },
      close () {
        this.dialogDepartment = false;
        this.dialogEmplyee = false;
        this.dialogJob = false;
        this.dialog = false;
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },
      closeDelete () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },
     async save () {
      console.log(this.editedIndex, 'dsadasdasdasdadfsdsadsadsadsadfsadsasadsadsads');
        if (this.editedIndex > -1) {
            if(this.dialogDepartment){
            var bodye ={
              departmentId: this.editedItem.departmentId,
            name: this.editedItem.name,
            location: this.editedItem.location,
            city: this.editedItem.city
            }
           await this.$store.dispatch('shifts/editDepartment', bodye).then(() => {
            this.initialize()
          });
          }else if(this.dialogEmplyee){
             const obj = {
              employeeId: this.editedItem.employeeId,
              fullName: this.editedItem.fullname,
              email: this.editedItem.email,
              phone: this.editedItem.phone,
              address: this.editedItem.address,
              // hireDate: "2023-06-12T14:44:17.559Z",
              salary:  parseInt(this.editedItem.salary),
              dailyHours:  parseInt(this.editedItem.dailyHours),
              leaveDays: parseInt(this.editedItem.leaveDays),
              departmentId:this.editedItem.departmentId,
              jobId: this.editedItem.jobId
              };// Output: string

           await this.$store.dispatch('users/editEmployee', obj).then(() => {
            this.initialize()
          });
          } else if (this.dialogJob){
             const obj = {
              jobId: this.editedItem.jobId,
              jobName: this.editedItem.jobName
              };// Outpu
             await this.$store.dispatch('users/editJob', obj).then(() => {
            this.initialize()
             });
          } else if(this.dialog){
             const obj = {
              managerId: this.editedItem.managerId,
              fullName: this.editedItem.fullname,
              email: this.editedItem.email,
              phone: this.editedItem.phone,
              address: this.editedItem.address,
              // hireDate: "2023-06-12T14:44:17.559Z",
              salary:  parseInt(this.editedItem.salary),
              dailyHours:  parseInt(this.editedItem.dailyHours),
              leaveDays: parseInt(this.editedItem.leaveDays),
              departmentId:this.editedItem.departmentId,
              jobId: this.editedItem.jobId
              };// Output: string

           await this.$store.dispatch('users/editManager', obj).then(() => {
            this.initialize()
          });
          }
        } else {
           if(this.dialogDepartment){
            var body ={
            name: this.editedItem.name,
            location: this.editedItem.location,
            city: this.editedItem.city
            }
           await this.$store.dispatch('shifts/addDepartment', body).then(() => {
            this.initialize()
          });
          }else if(this.dialogEmplyee){
             const obj = {
              fullName: this.editedItem.fullname,
              email: this.editedItem.email,
              phone: this.editedItem.phone,
              address: this.editedItem.address,
              // hireDate: "2023-06-12T14:44:17.559Z",
              salary:  parseInt(this.editedItem.salary),
              dailyHours:  parseInt(this.editedItem.dailyHours),
              leaveDays: parseInt(this.editedItem.leaveDays),
              departmentId:this.editedItem.departmentId,
              jobId: this.editedItem.jobId
              };// Output: string

           await this.$store.dispatch('users/addEmployee', obj).then(() => {
            this.initialize()
          });
          } else if (this.dialogJob){
             const obj = {
              jobName: this.editedItem.jobName
              };// Outpu
             await this.$store.dispatch('users/addJob', obj).then(() => {
            this.initialize()
             });
          } else if(this.dialog){
             const obj = {
              fullName: this.editedItem.fullname,
              email: this.editedItem.email,
              phone: this.editedItem.phone,
              address: this.editedItem.address,
              // hireDate: "2023-06-12T14:44:17.559Z",
              salary:  parseInt(this.editedItem.salary),
              dailyHours:  parseInt(this.editedItem.dailyHours),
              leaveDays: parseInt(this.editedItem.leaveDays),
              departmentId:this.editedItem.departmentId,
              jobId: this.editedItem.jobId
              };// Output: string

           await this.$store.dispatch('users/addManager', obj).then(() => {
            this.initialize()
          });
          }
          // 
        } 
        this.close()
      },
    },
  }
</script>