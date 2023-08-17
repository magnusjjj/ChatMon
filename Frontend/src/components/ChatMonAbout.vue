<script setup>
    import { CloseOutlined } from '@vicons/antd';
    import { defineEmits } from 'vue';
    
    const emit = defineEmits(["Close"]);
    const props = defineProps({show: Boolean});


    function closewindow(){{
        emit("Close");
    }}
</script>

<template>
    <div class="aboutwindow" v-if="props.show">
        <div class="scrollboxoutside">
            <div class="scrollboxinside">
                <ChatMonLogo />
                <h2>made by <a href="https://twitch.tv/slightlytango" target="_blank">SlightlyTango</a></h2>
                <h3>based on an idea by and commisioned for <a href="https://twitch.tv/skylordzoey" target="_blank">SkylordZoey</a></h3>

                Becoming a patron of SlightlyTango really helps, if you have the ability.<br />
                And if you do, in the next update you get listed here. ;)<br />
                <br />
                <a href="https://www.patreon.com/SlightlyTango" target="_blank">
                    <img src="/assets/becomeapatron.png" />
                </a>
                <br />
                <br />

                Licensed under the MIT license, apart from assets and libraries, which are licensed as below.
                record.svg and settings.svg are from google material, and are licensed under Apache 2.0.<br />
                pokeball.svg is made by Andreuvv from wikipedia, and is licensed under Creative Commons Attribution-Share Alike 4.0 International.<br />
                The pokemon font file is tagged as being made by Creative Commons Attribution Share Alike , made by KaelinT.<br />
                You can find the licenses for the javascript libraries used in <a href="/chunk-vendors.licenses.txt" target="_blank">this text file</a>.<br/>
                <br/>
                You can find the source code for ChatMon at <a href="https://github.com/magnusjjj/ChatMon" target="_blank">the github.</a>

                <n-button size="large" @click="closewindow" type="primary">
                    Close
                    <template #icon>
                        <n-icon><close-outlined /></n-icon>
                    </template>
                </n-button>
            </div>
        </div>
    </div>
</template>

<style scoped>
    button {
        margin-left: 1em;
    }

    .aboutwindow {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%; 
        background-color: rgba(0,0,0,0.5);
        z-index: 99999;

    }
    .scrollboxoutside {
        background-color: white;
        width: 80%;
        height: 60%; /* I don't know why this is calculated wrong, but here we are */
        position: relative;
        top: 5%;
        left: 5%;
        padding: 1em;
        border-radius: 1em;
    }

    .scrollboxinside {
        overflow-y: scroll;
        width: 100%;
        height: 100%;
    }
</style>