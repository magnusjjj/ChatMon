<script setup>
    import { ref } from 'vue';
    import ChatHandler from '../handlers/chathandler.js';

    const props = defineProps({
        message: String
    })

    var message = ref(props.message);

    ChatHandler.onStatus((statusmessage) => {
        message.value = statusmessage;
    });
</script>

<template>
    <div class="message">
        {{message}}
    </div>
</template>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .message {background-color: white;}
</style>

<style>

</style>