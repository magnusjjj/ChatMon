<script setup>
    const props = defineProps({
        title: String
    })
</script>

<template>
    <n-modal v-show="true" :block-scroll="false">
        <n-card style="max-width: 80%; max-height: 60%; overflow-y: scroll; position: fixed; top: 5%; left: 10%;" :title="props.title">
            <template #header-extra>
                <slot name="header"></slot>
            </template>
            <slot></slot>
            <template #action>
                <slot name="action"></slot>
            </template>
        </n-card>
    </n-modal>
</template>

<!--<template>
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
-->