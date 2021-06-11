<template>
	<div class="wrapper">
		<v-head></v-head>
		<v-sidebar></v-sidebar>
		<div class="content-box" :class="{'content-collapse':collapse}"
			style="border-left:1px solid #dcdfe5 !important;">
			<div style="height: 30px;width: 100%;padding-top: 3px;">
				<v-tags></v-tags>
			</div>
			<transition name="move" mode="out-in">
				<keep-alive :include="tagsListNames">
					<router-view></router-view>
				</keep-alive>
			</transition>
		</div>
	</div>
</template>

<script>
	import vHead from './Header.vue';
	import vSidebar from './Sidebar.vue';
	import vTags from "./TagsView";
	import { mapState } from 'vuex'
	import promission from "./../../router/promission.js";
	const tagAndTagSpacing = 4
	export default {
		name: 'Home',
		data() {
			return {
				tagsList: this.$store.state.TabKeepList,
				left: 0
			}
		},
		methods: {

		},
		created() {

		},
		components: {
			vHead, vSidebar,vTags
		},
		computed: {
			...mapState([
				'collapse'
			]),
			tagsListNames(){
				return this.tagsList.map(z=>z.name);
			}
		},
		watch: {
			
		}
	}
</script>