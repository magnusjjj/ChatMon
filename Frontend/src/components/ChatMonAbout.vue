<script setup>
    import { NButton, NIcon } from 'naive-ui'
    import { QuestionOutlined, CloseOutlined } from '@vicons/antd';
    import { ref } from 'vue';
    
    var windowopen = ref(false);

    function openwindow(){{
        windowopen.value = true;
    }}

    function closewindow(){{
        windowopen.value = false;
    }}
</script>

<template>
    <n-button size="large" @click="openwindow" type="primary">
        About
        <template #icon>
            <n-icon><question-outlined /></n-icon>
        </template>
    </n-button>
    <div class="aboutwindow" v-if="windowopen">
        <h2>ChatMon, made by <a href="https://twitch.tv/slightlytango">SlightlyTango</a></h2>
        <h3>based on an idea by and commisioned for <a href="https://twitch.tv/skylordzoey">SkylordZoey</a></h3>
        Licensed under the MIT license, apart from assets and libraries, which are licensed as below.
        <br />
        <br />
        record.svg and settings.svg are from google material, and are licensed under Apache 2.0.<br />
        pokeball.svg is made by Andreuvv from wikipedia, and is licensed under Creative Commons Attribution-Share Alike 4.0 International.<br />
        The pokemon font file is tagged as being made by Creative Commons Attribution Share Alike , made by KaelinT.<br />
        You can find the licenses for the javascript libraries used in <a href="/chunk-vendors.licenses.txt" target="_blank">this text file</a>.

        <n-button size="large" @click="closewindow" type="primary">
            Close
            <template #icon>
                <n-icon><close-outlined /></n-icon>
            </template>
        </n-button>
    </div>
</template>

<style scoped>
    button {
        margin-left: 1em;
    }

    .aboutwindow {
        position: fixed;
        top: 1em;
        left: 1em;
        width: 80%;
        height: 80%;
        background-color: rgba(255,255,255,0.9);
        z-index: 99999;
    }
</style>