<template>
    <div class="preloader">
        <div class="logo">
            <h1>ChatMon</h1>
            <img src="assets/pokeball.svg" class="pokeball" />
        </div>
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
    import { NProgress } from 'naive-ui';
    import WebView2IntegrationHandler from '../handlers/webview2integrationhandler';

    const message = ref("");
    const percentage = reactive([0, 0]);


    WebView2IntegrationHandler.onMessage('ChatMonProgress', (text) => {
        percentage[0] = text.percent1;
        percentage[1] = text.percent2;
        message.value = text.message;
    });
</script>

<style scoped>
    .logo {
        position: absolute;
        width: 90%;
        height: 20%;
        left: 5%;
        top: 5%;
        overflow: hidden;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .logo h1 {
        float: left;
        display: inline;
        margin: 0;
        vertical-align: middle;
        font-family: "Pokemon R/S", Arial, Helvetica, sans-serif;
        font-size: 7em;
        text-shadow: 0.00em 0.00em 0.10em #CCCCCCCC;
    }

    @keyframes wiggleball {
        0% {
            transform: rotate(-25deg);
        }

        50% {
            transform: rotate(25deg);
        }

        100% {
            transform: rotate(-25deg);
        }
    }

    .pokeball {
        float: left;
        width: 20%;
        height: auto;
        max-height: 100%;
        display: inline-block;
        vertical-align: middle;
        margin-left: -4em;
        z-index: -1;
        animation-name: wiggleball;
        animation-duration: 4s;
        animation-iteration-count:infinite;
    }

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