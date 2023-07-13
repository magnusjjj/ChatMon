<script setup>
    import { NInput, NTooltip, NForm, NFormItem, NInputGroup, NButton, NIcon, NSlider, NCheckbox, NSelect } from 'naive-ui'
    import { SoundFilled } from '@vicons/antd';
    import { reactive } from 'vue';
    import SaveDataHandler from '../handlers/savedatahandler.js';
    import keylist from '../keyset.json';

    const emit = defineEmits(["SaveCompleted"]);

    const props = defineProps({
        show: Boolean
    })

    var savedata = reactive({
        voice_level: 50,
        key: 0,
        key_alt: false,
        key_ctrl: false,
        channel: ''
    });

    SaveDataHandler.onSettingsChanged(function (settings) {
        Object.assign(savedata, settings);
    });

    var validation = reactive({});

    SaveDataHandler.onValidationError((errors) => {
        Object.assign(validation, errors);
    });

    function Save() {
        for (const prop in validation) {
            validation[prop] = "";
        }

        console.log(validation);

        emit("SaveCompleted", savedata);
        SaveDataHandler.SaveSettings(savedata);
    }

    function testVoice() {
        var utterThis = new SpeechSynthesisUtterance("I'm a pokemon!");
        utterThis.volume = (savedata.voice_level ?? 0) / 100;
        window.speechSynthesis.speak(utterThis);
    }

    function onchannelnamechange() {
        var filteredvalue = savedata.channel.replace(/(http|https):\/\/[^/]*\/([^?/#]*)/g, "$2").toLowerCase();
        savedata.channel = filteredvalue;
        console.log("Found an url");
    }

    var filtered_keylist = keylist.map((row) => { return { "label": row[0] + "-" + row[2], "value": row[1] } });

</script>

<template>
    <div class="configurator" v-if="props.show">
        <div class="centeredwindow">
            <n-form :show-feedback="true">
                <n-form-item label="Twitch channel name" :feedback="validation.channel" :validation-status="validation.channel ? 'warning' : ''">
                    <n-tooltip trigger="hover">
                        <template #trigger>
                            <n-input v-model:value="savedata.channel" @blur="onchannelnamechange" />
                        </template>
                        The twitch channel. You can paste in your url.
                    </n-tooltip>
                </n-form-item>
                <n-form-item label="TTS Voice level">
                    <n-input-group>
                        <n-slider v-model:value="savedata.voice_level" :step="1" />
                        <n-button @click="testVoice">
                            Test
                            <template #icon>
                                <n-icon><sound-filled /></n-icon>
                            </template>
                        </n-button>
                    </n-input-group>
                </n-form-item>
                <n-form-item label="TTS Chatmessage Prefix"  :feedback="validation.speak_prefix" :validation-status="validation.speak_prefix ? 'warning' : ''">
                    <n-tooltip trigger="hover">
                        <template #trigger>
                            <n-input v-model:value="savedata.speak_prefix" />
                        </template>
                        The twitch channel
                    </n-tooltip>
                </n-form-item>
                
                <n-form-item label="Shutup keys" >
                    <div>
                        <n-checkbox v-model:checked="savedata.key_ctrl">Ctrl</n-checkbox>
                        <n-checkbox v-model:checked="savedata.key_alt">Alt</n-checkbox>
                    </div>
                    <n-select filterable
                              placeholder="Select key"
                              v-model:value="savedata.key"
                              :options="filtered_keylist" />
                </n-form-item>
                <n-form-item>
                    <n-button @click="Save">Save</n-button>
                </n-form-item>
            </n-form>
        </div>
    </div>
</template>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .configurator {
        width: 100%;
        position: absolute;
        top: 0;
        left: 0;
        background-color: rgb(255, 255, 255, 0.5);
        z-index: 1000;
    }

    .configurator label {
            display: block;
    }

    .configurator input {
        display: block;
        width: 100%;
    }

    select {
        display: block;
        width: 100%;
    }

            .centeredwindow {
        width: 70%;
        margin-left: auto;
        margin-right: auto;
        background-color: white;
    }


</style>

<style>
    .n-slider .n-slider-rail {
        top: 35%;
        width: 92%;
        left: 5%;
    }
</style>