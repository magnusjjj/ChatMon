<template>
    <div class="preloader">
        <ChatMonLogo/>
        <div class="loadingstatus">
            <n-progress type="multiple-circle"
                        :stroke-width="6"
                        :circle-gap="0.5"
                        :percentage=percentage
                        :color="[
          '#FF0000',
          '#FF0000',
        ]"
                        :rail-style="[
          { stroke: '#FF0000', opacity: 0.3 },
          { stroke: '#FF0000', opacity: 0.3 },
        ]" style="width: 150px">
                <div style="text-align: center">
                    {{percentage[1]}}%
                </div>
            </n-progress>
            <div class="message">
                {{message}}
            </div>
        </div>
    </div>
</template>

<script setup>
    import {ref, reactive} from 'vue';
    import WebView2IntegrationHandler from '../handlers/webview2integrationhandler';

    const message = ref("");
    const percentage = reactive([0, 0]);

    // We are listening to the C# backend to update the progress bar
    WebView2IntegrationHandler.onMessage('ChatMonProgress', (text) => {
        percentage[0] = text.percent1;
        percentage[1] = text.percent2;
        message.value = text.message;
    });
</script>

<style scoped>
    .preloader {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: white;
    }

    .loadingstatus {
        position: absolute;
        width: 90%;
        height: 40%;
        left: 5%;
        top: 25%;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
    }

    .message {
        text-align: center;
        margin-top: 1em;
    }
</style>