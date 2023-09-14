<template>
    <div class="configurator" v-if="props.show">
        <div class="centeredwindow">
            <slot></slot>
        </div>
    </div>
</template>

<script setup>
    const props = defineProps({
        show: Boolean
    })
</script>

<style scoped>
    .configurator {
        width: 100%;
        height: 100%;
        position: absolute;
        top: 0;
        left: 0;
        background-color: rgb(0, 0, 0, 0.5);
        z-index: 1000;
    }

    .centeredwindow {
        width: 70%;
        margin-left: auto;
        margin-right: auto;
        background-color: white;
        padding: 1em;
        border-radius: 1em;
        padding-bottom: 0.5em;
    }
</style>