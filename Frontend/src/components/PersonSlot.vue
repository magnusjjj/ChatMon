<template>
    <div class="character">
        <div class="characterpicture" @click="ShowConfigurator(slotnumber)">
            <img :src="image" />
            <div class="material-icons charactersettings"></div>
        </div>
        <div class="nametag">
            <div v-for="(displayname, index) in person.displaynames" v-bind:key="index">
                {{displayname}}
            </div>
        </div>
        <div class="message" v-if="shouldshowmessage">{{message}}</div>
    </div>
</template>

<script setup>
    import { ref, onUpdated, watch, onMounted } from 'vue';
    import GameHandler from '../handlers/gamehandler';

    const props = defineProps({
        person: Object,
        slotnumber: Number,
        message: String
    });

    var message = ref(props.message);

    const image = ref("");

    const emit = defineEmits(["CharacterPictureClicked"]);


    function ShowConfigurator(slotnumber) {
        emit("CharacterPictureClicked", slotnumber);
    }

    var shouldshowmessage = ref(false);
    var hidingtimeout = null;

    watch(() => props.message, (oldvalue, newvalue) => {
        console.log(oldvalue, newvalue);
        clearTimeout(hidingtimeout);
        hidingtimeout = setTimeout(() => { shouldshowmessage.value = false; }, 10000);

        message.value = props.message;
        shouldshowmessage.value = message.value != "";
    });

    onMounted(async () => {
        image.value = await GameHandler.currentGame().resolveImage(props.person.pokemon);
    });

    onUpdated(async () => {
        console.log("Updating person!");
        image.value = await GameHandler.currentGame().resolveImage(props.person.pokemon);
        /*console.log("Updating slot");
        clearTimeout(hidingtimeout);
        hidingtimeout = setTimeout(() => { shouldshowmessage.value = false; }, 10000);
        
        message.value = props.message;
        shouldshowmessage.value = message.value != "";*/
    });
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .message {
        border: 0.2em solid #DDDDDD;
        border-radius: 2em;
        box-shadow: 0.5em 0.5em 0.5em #AAA;
        background-color: #EEEEEE;
        padding: 0.5em;
        font-family: "Pokemon R/S", Arial, Helvetica, sans-serif;
        font-size: 1.5em;
    }

    .nametag {
        border: 0.2em solid #DDDDDD;
        border-radius: 2em;
        background-color: #EEEEEE;
        padding: 0.5em;
        font-family: "Pokemon R/S", Arial, Helvetica, sans-serif;
        font-size: 1.5em;
        text-align: center;
    }

    .character {
        width: calc(100% / 6);
        float: left;
        position: relative;
    }

    .characterpicture {
        position: relative;
    }

    .characterpicture img {
        width: 100%;
        height: auto;
        max-height: 142px;
        image-rendering: pixelated;
    }

    .characterpicture img[src=""] {
        width: 100%;
        height: auto;
        min-height: 142px;
        image-rendering: pixelated;
    }

    .charactersettings {
        display: none;
        position: absolute;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        background-image: url("/assets/settings.svg");
        background-repeat: no-repeat;
        background-size: 100%;
    }

    .character:hover .charactersettings {
        display: block;
    }

    /* we will explain what these classes do next! */
    .v-enter-active,
    .v-leave-active {
        transition: opacity 0.5s ease;
    }

    .v-enter-from,
    .v-leave-to {
        opacity: 0;
    }
</style>