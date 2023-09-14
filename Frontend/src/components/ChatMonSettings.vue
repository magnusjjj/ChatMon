<template>
    <ChatMonWindow :show="props.show">
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
            <n-form-item label="TTS Chatmessage Prefix" :feedback="validation.speak_prefix" :validation-status="validation.speak_prefix ? 'warning' : ''">
                <n-tooltip trigger="hover">
                    <template #trigger>
                        <n-input v-model:value="savedata.speak_prefix" />
                    </template>
                    The twitch channel
                </n-tooltip>
            </n-form-item>
            <n-form-item label="Shutup keys">
                <div>
                    <n-checkbox v-model:checked="savedata.key_ctrl">Ctrl</n-checkbox>
                    <n-checkbox v-model:checked="savedata.key_alt">Alt</n-checkbox>
                </div>
                <div>
                    <n-select filterable
                                placeholder="Select key"
                                v-model:value="savedata.key"
                                placement="top"
                                :options="filtered_keylist" />
                </div>

            </n-form-item>
            <div style="margin-top: -1em; margin-bottom: -1.5em;">If the keybind doesn't seem to work, try other keybinds. Some, like for instance *just* binding F12, are blocked by the operating system or other applications.</div>
            <n-form-item>
                <n-space>
                    <n-button @click="Save" type="primary">Save</n-button>
                    <n-button @click="Cancel">Cancel</n-button>
                </n-space>
            </n-form-item>
        </n-form>
    </ChatMonWindow>
</template>

<script setup>
    import { SoundFilled } from '@vicons/antd';
    import { reactive, onUpdated } from 'vue';
    import SaveDataHandler from '../handlers/savedatahandler.js';
    import keylist from '../keyset.json';

    const emit = defineEmits(["SaveCompleted", "Cancelled"]);

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

    var settingscopy = {};
    SaveDataHandler.onSettingsChanged(function (settings) {
        settingscopy = settings;
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

    function Cancel() {
        emit("Cancelled");
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

    onUpdated(() => {
        Object.assign(savedata, settingscopy);
    });

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
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
</style>

<style>
    .n-slider .n-slider-rail {
        top: 35%;
        width: 92%;
        left: 5%;
    }
</style>