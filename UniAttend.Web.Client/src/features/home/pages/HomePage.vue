<template>
  <!-- Hero Section with Animation -->
  <div class="bg-gradient-to-br from-indigo-50 via-white to-blue-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 pt-24 pb-20">
      <div class="text-center animate-fade-in">
        <h1 class="text-5xl tracking-tight font-extrabold text-gray-900 sm:text-6xl md:text-7xl">
          <span class="block">Transform Attendance</span>
          <span class="block text-indigo-600 mt-2">with UniAttend</span>
        </h1>
        <p class="mt-6 max-w-2xl mx-auto text-lg text-gray-600 sm:text-xl">
          Modern attendance tracking system designed for universities. 
          Streamline your process with RFID cards and mobile check-ins.
        </p>
        <div class="mt-8 max-w-md mx-auto sm:flex sm:justify-center">
          <router-link to="/login"
            class="inline-flex items-center px-8 py-4 border border-transparent text-base font-medium rounded-xl text-white 
            bg-indigo-600 hover:bg-indigo-700 transition-all duration-200 transform hover:scale-105 shadow-lg">
            Login
          </router-link>
        </div>
      </div>
    </div>

    <!-- Statistics Section -->
    <div class="bg-white py-16">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-4">
          <div class="text-center">
            <div class="text-4xl font-bold text-indigo-600">99.9%</div>
            <div class="mt-2 text-gray-600">Accuracy Rate</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-indigo-600">50+</div>
            <div class="mt-2 text-gray-600">Universities</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-indigo-600">1M+</div>
            <div class="mt-2 text-gray-600">Check-ins</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-indigo-600">24/7</div>
            <div class="mt-2 text-gray-600">Support</div>
          </div>
        </div>
      </div>
    </div>

    <!-- Features Section -->
    <div class="bg-gray-50 py-16">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <h2 class="text-3xl font-bold text-center text-gray-900 mb-12">
          Smart Features for Modern Education
        </h2>
        <div class="grid grid-cols-1 gap-8 md:grid-cols-3">
          <!-- RFID Card Feature -->
          <div class="bg-white rounded-xl p-8 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-1">
            <div class="w-14 h-14 mx-auto bg-indigo-100 rounded-xl flex items-center justify-center">
              <span class="material-icons text-2xl text-indigo-600">credit_card</span>
            </div>
            <h3 class="mt-6 text-xl font-semibold text-gray-900 text-center">RFID Card Check-in</h3>
            <p class="mt-4 text-gray-600 text-center leading-relaxed">
              Seamlessly track attendance using RFID cards. Quick tap-and-go system 
              integrated with classroom terminals for instant verification.
            </p>
          </div>

          <!-- Mobile Check-in Feature -->
          <div class="bg-white rounded-xl p-8 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-1">
            <div class="w-14 h-14 mx-auto bg-indigo-100 rounded-xl flex items-center justify-center">
              <span class="material-icons text-2xl text-indigo-600">smartphone</span>
            </div>
            <h3 class="mt-6 text-xl font-semibold text-gray-900 text-center">Mobile Check-in</h3>
            <p class="mt-4 text-gray-600 text-center leading-relaxed">
              Check in using your mobile device with secure OTP verification. 
              Flexible attendance marking from anywhere within the campus network.
            </p>
          </div>

          <!-- Real-time Tracking Feature -->
          <div class="bg-white rounded-xl p-8 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-1">
            <div class="w-14 h-14 mx-auto bg-indigo-100 rounded-xl flex items-center justify-center">
              <span class="material-icons text-2xl text-indigo-600">analytics</span>
            </div>
            <h3 class="mt-6 text-xl font-semibold text-gray-900 text-center">Real-time Tracking</h3>
            <p class="mt-4 text-gray-600 text-center leading-relaxed">
              Monitor attendance records in real-time. Automatic notifications for absence thresholds 
              and comprehensive reporting system.
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 1s ease-in;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>