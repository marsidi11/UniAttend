import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';
import './assets/styles/tailwind.css';
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";

const app = createApp(App);
const pinia = createPinia();

const toastOptions = {
    position: "top-right",
    timeout: 5000,
    closeOnClick: true,
    pauseOnFocusLoss: true,
    pauseOnHover: true,
    draggable: true,
    draggablePercent: 0.6,
    showCloseButtonOnHover: false,
    hideProgressBar: false,
    closeButton: "button",
    icon: true,
    rtl: false
  };
  
  app.use(Toast, toastOptions);

app.use(pinia);
app.use(router);
app.mount('#app');