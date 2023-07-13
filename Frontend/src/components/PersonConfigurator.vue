<script setup>
    import { NSelect, NInput, NTooltip, NForm, NFormItem, NInputGroup, NButton, NIcon } from 'naive-ui'
    import { SoundFilled } from '@vicons/antd'
    import { ref, reactive, onUpdated } from 'vue';
    import SaveDataHandler from '../handlers/savedatahandler.js';
    import TTSHandler from '../handlers/ttshandler.js';

    //import { defineComponent } from 'vue';
    const props = defineProps({
        person: Object,
        show: Boolean,
        personslot: Number
    });

    const emit = defineEmits(["SaveCompleted"]);

    const state = reactive({
        voicelist: TTSHandler.getNaiveVoiceList(),
    });

    var savedata = reactive({
        username: ref(props.person.username),
        pokemon: ref(props.person.pokemon ?? ""),
        voice: ref(props.person.voice ?? ""),
        displayname: ref(props.person.displayname ?? "")  
    });

    function Save() {
        if (props.person.username != savedata.username) {
            savedata.displayname = savedata.username;
        }
        SaveDataHandler.SaveCharacter(props.personslot, savedata);
        emit("SaveCompleted", props.personslot, savedata);
    }

    const pokedex = ref([]);

    fetch('/game/pokemon/pokemon.json-master/pokedex.json')
        .then(response => response.json())
        .then(data => {
            pokedex.value = data.map(line => {
                return ({ "value": line.id, "label": line.name.english });
            });
            pokedex.value.unshift({ "value": "0", "label": "None" });
        });

/*
    var synth = window.speechSynthesis;
    var voices = synth.getVoices();

    function populatevoices() {
        voices = synth.getVoices();

        for (let i = 0; i < voices.length; i++) {
            var langtext = voices[i].default ? "default" : `${voices[i].name} (${voices[i].lang})`;
            state.voicelist.push({ "value": langtext, "label": langtext });
        }
        console.log("Loaded speech data!", state.voicelist);
    }

    if (voices.length) populatevoices(state);
    synth.addEventListener("voiceschanged", () => { populatevoices() });*/

    onUpdated(() => {
        savedata.username = props.person.username ?? "";
        savedata.pokemon = props.person.pokemon ?? "0";
        savedata.voice = props.person.voice ?? "";
        savedata.displayname = props.person.displayname ?? "";
    });


    function testVoice() {
        TTSHandler.speak("I'm a pokemon!", savedata.voice);
/*        var utterThis = new SpeechSynthesisUtterance("I'm a pokemon!");

        for (let i = 0; i < voices.length; i++) {
            if (`${voices[i].name} (${voices[i].lang})` == savedata.voice) {
                utterThis.voice = voices[i];
            }
        }

        utterThis.volume = 0.1; // Todo, actually grab the setting
        synth.speak(utterThis);*/
    }

    function onusernamechange() {
        savedata.username = savedata.username.toLowerCase();
    }
</script>

<template>
    <div class="configurator" v-if="show">
        <div class="centeredwindow">
            <n-form :show-feedback="false">
                <n-form-item label="Username">
                    <n-tooltip trigger="hover">
                        <template #trigger>
                            <n-input v-model:value="savedata.username" @blur="onusernamechange"/>
                        </template>
                        The *username* of the chatter, not what it looks like in chat. Usually lowercase and no special letters. You might have to ask. You can also begin the line with !, like !bulba for a slot that anyone in chat can use with that command.
                    </n-tooltip>
                </n-form-item>
                <n-form-item label="Pokemon">
                    <n-select filterable
                              placeholder="Select a pokemon"
                              v-model:value="savedata.pokemon"
                              :options="pokedex" />
                </n-form-item>
                <n-form-item label="Voice">
                    <n-input-group>
                        <n-select filterable
                                  placeholder="Select voice"
                                  v-model:value="savedata.voice"
                                  :options="state.voicelist" />
                        <n-button @click="testVoice">
                            Test
                            <template #icon>
                                <n-icon><sound-filled /></n-icon>
                            </template>
                        </n-button>
                    </n-input-group>

                </n-form-item>
                <!--            -->
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