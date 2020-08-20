var deleteOrgPopUp = new Vue({
    el: '#deleteConfirmPopUp',
    data: {
        Deleting: false,
        DeleteResult: false,
        url_delete: '/OrganizationInfo/Remove',
    },
    methods: {
        DeleteCombineConfirmed: function () {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('id', org_menu.org_id);

            axios.post(this.url_delete, null, {
                params: params
            }).then(response => {
                var str = response.data.toString();
                if (str.indexOf("<!--This is the login layout-->") == 0) {
                    alert("The system detects that you have not operated for a long time, please login again");
                    document.clear();
                    location.reload();
                }
                else {
                    if (response.data.Success) {
                        DeleteResult = true;
                        this.RedirectToOrg();
                        SuccessTurnToParentPage("Success", true);
                    }
                    else {
                        SuccessTurnToParentPage(response.data.Reason, false);
                    }
                }
            }).catch(error => {
                $("#body-content").html(error.response);
            });

            this.Deleting = false;

            $('#loading-panel').attr('hidden', 'hidden');
        },
        DeleteCancelled: function () {
            deleteOrgPopUp.Deleting = false;
        },
        RedirectToOrg: function () {
            if (DeleteResult) {
                axios.get('/Organizations/Organizations')
                    .then(response => {
                        var str = response.data.toString();
                        if (str.indexOf("<!--This is the login layout-->") == 0) {
                            alert("The system detects that you have not operated for a long time, please login again");
                            document.clear();
                            location.reload();
                        }
                        else {
                            $("#body-content").html(response.data);
                        }
                    })
                    .catch(error => {
                        $("#body-content").html(error.response);
                    });

                DeleteResult = false;
            }
        },
    },
    computed: {
        Show: function () {
            if (!this.Deleting) {
                this.Combine_Id = 0;
                this.event = null;
            }
            return this.Deleting;
        }
    }
});

var org_menu = new Vue({
    el: '#org-menu',
    data: {
        org_id: '',
        url_edit: '/OrganizationInfo/Edit',
    },
    mounted() {
        this.org_id = $('#org-menu').attr('data-org-id');
    },
    methods: {
        DeleteOrg: function () {
            deleteOrgPopUp.Deleting = true;
        },
        EditOrg: function () {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('org_id', this.org_id);

            axios.get(this.url_edit, {
                params: params
            }).then(response => {
                var str = response.data.toString();
                if (str.indexOf("<!--This is the login layout-->") == 0) {
                    alert("The system detects that you have not operated for a long time, please login again");
                    document.clear();
                    location.reload();
                }
                else {
                    if (response.data.Result != undefined && !response.data.Result && response.data.Reason != undefined) {
                        SuccessTurnToParentPage(response.data.Reason, false);
                    }
                    else {
                        $("#body-content").html(response.data);
                    }
                }
            }).catch(error => {
                $("#body-content").html(error.response);
            });

            $('#loading-panel').attr('hidden', 'hidden');
        }
    }
});

Vue.component('team-list-item', {
    template: '\
    <div class="list-item" @click="getTeamInfo(team_id)">\
        <div class="row pointer">\
            <div class="col-lg-2 col-md-3 col-sm-2 list-item-img-block">\
                <img v-bind:src="img_path" />\
            </div>\
            <div class="col-lg-10 offset-lg-0 col-md-9 offset-md-0 col-12 offset-0 pb-2">\
                <h3 class="text-FXV mt-2">{{name}}</h3>\
                <div class="row text-white">\
                    <p class="col-xl-3 col-lg-4 col-6">\
                        <span class="material-icons mr-1" style="float:left;line-height: 19px;font-size: 19px;">\
                            location_on\
                        </span> {{location}}\
                    </p>\
                    <p class=" col-xl-3 col-lg-4 col-6">\
                        <span class="material-icons mr-1" style="float:left;line-height: 19px;font-size: 19px;">\
                            person\
                        </span> {{manager}}\
                    </p>\
                    <p class=" col-xl-3 col-lg-4 col-6">\
                        <span class="text-muted">\
                            Members:\
                        </span> {{num_members}}\
                    </p>\
                </div>\
            </div>\
        </div>\
    </div>\
      ',
    props: ['team_id', 'img_path', 'name', 'location', 'manager', 'num_members'],
    methods: {
        getTeamInfo: function (team_id) {
            $('#loading-panel').removeAttr('hidden');
            window.location.href = "/OrganizationTeamInfo/Index?id=" + team_id;
            $('#loading-panel').attr('hidden', 'hidden');
        }
    }
});
var vueTeamsMenu = new Vue({
    el: "#teams-menu",
    data: {
        url_Search: '/api/GetTeamsByName',
        url_AddTeam: '/OrganizationInfo/Create',
        SearchText: '',
    },
    watch: {
        SearchText: 'CheckSearchText',
    },
    methods: {
        CheckSearchText: function () {
            this.SearchText = this.SearchText.toString().replace(/[\\/<>'"]/g, '');
        },
        Search: _.debounce(
            function () {
                $('#teams-list').find('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                vueTeamsList.lists = [];

                if (this.SearchText == "") {
                    vueTeamsList.last_id = 0;
                    vueTeamsList.hasMore = true;
                    vueTeamsList.LoadMore();
                }
                else {
                    this.SendSearchRequest();
                }
            }, 500),
        SendSearchRequest: function () {
            axios.get(this.url_Search + '/' + this.SearchText + '/' + org_menu.org_id)
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        try {
                            var json = JSON.parse(response.data);
                            $.each(json, function (index, item) {
                                vueTeamsList.lists.push({
                                    id: vueTeamsList.nextTodoId++,
                                    team_id: item.TeamId,
                                    img_path: item.Img_Path,
                                    name: item.Name,
                                    location: item.Location,
                                    manager: item.ManagerName,
                                    num_members: item.NumberOfMembers
                                });
                            });

                            $('#teams-list').find('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                            vueTeamsList.hasMore = false;
                        }
                        catch (e) {
                            $("#body-content").html(response.data);
                        }
                    }
                })
                .catch(error => {
                    $("#body-content").html(error.response);
                });
        },
    }
});

var vueTeamsList = new Vue({
    el: "#teams-list",
    data: {
        last_id: 0,
        num: 10,
        url_TeamList: '/api/GetTeamList',
        url_TeamInfo: '/OrganizationTeamInfo/OrganizationTeamInfo',
        lists: [],
        nextTodoId: 0,
    },
    mounted() {
        this.LoadMore();
    },
    methods: {
        LoadMore: function () {
            $('#teams-list').find('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
            axios.get(this.url_TeamList + '/' + org_menu.org_id)
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        try {
                            var json = JSON.parse(response.data);
                            $.each(json, function (index, item) {
                                vueTeamsList.lists.push({
                                    id: vueTeamsList.nextTodoId++,
                                    team_id: item.TeamId,
                                    img_path: item.Img_Path,
                                    name: item.Name,
                                    location: item.Location,
                                    manager: item.ManagerName,
                                    num_members: item.NumberOfMembers
                                });
                            });

                            if (json.length < 10) {
                                $('#teams-list').find('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                                vueTeamsList.hasMore = false;
                            }
                            else {
                                $('#teams-list').find('#more').removeAttr('disabled').removeClass('text-muted').addClass('text-white').html("More...");
                                vueTeamsList.hasMore = true;
                            }
                        }
                        catch (e) {
                            $("#body-content").html(response.data);
                        }
                    }
                })
                .catch(error => {
                    $("#body-content").html(error.response);
                });
        },
    },
});


Vue.component('member-list-item', {
    template: '\
    <div class="list-item">\
        <div class="row pointer" v-on:click="$emit(\'get_profile\')">\
            <div class="col-lg-2 col-md-3 col-sm-2 list-item-img-block">\
                <img v-bind:src="img_path" />\
            </div>\
            <div class="col-lg-10 offset-lg-0 col-md-9 offset-md-0 col-12 offset-0 pb-2">\
                <h4 class="text-FXV mt-2">{{name}} <span v-bind:class="gender"></span> <span>{{age}}</span></h4>\
                <div class="row text-white pt-2">\
                    <p class="col-xl-3 col-lg-4 col-md-5 col-12 mb-1">\
                        <span class="text-muted">\
                            Role:\
                        </span> {{position}}\
                    </p>\
                    <p class="col-xl-9 col-lg-8 col-md-7 col-12 mb-1">\
                        <span class="text-muted">\
                            Team:\
                        </span> {{team_name}}\
                    </p>\
                </div>\
            </div>\
        </div>\
    </div>\
      ',
    props: ['img_path', 'name', 'gender', 'age', 'position', 'team_name', 'uid'],
});

var vueMembersMenu = new Vue({
    el: "#members-menu",
    data: {
        url_Search: '/api/GetUsersByOrgId_UserName',
        SearchText: '',
    },
    watch: {
        SearchText: 'CheckSearchText'
    },
    methods: {
        CheckSearchText: function () {
            this.SearchText = this.SearchText.toString().replace(/[\\/<>'"]/g, '');
        },
        Search: _.debounce(
            function () {
                $('#members-list').find('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                vueMembersList.lists = [];

                if ($('#members-menu input').val() == "") {
                    vueMembersList.last_id = 0;
                    vueMembersList.hasMore = true;
                    vueMembersList.LoadMore();
                }
                else {
                    this.SendSearchRequest();
                }
            }, 500),
        SendSearchRequest: function () {
            axios.get(this.url_Search + '/' + org_menu.org_id + '/' + this.SearchText)
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        try {
                            var json = JSON.parse(response.data);
                            $.each(json, function (index, item) {
                                vueMembersList.lists.push({
                                    id: vueTeamsList.nextTodoId++,
                                    uid: item.UId,
                                    img_path: item.Img_Path,
                                    name: item.Name,
                                    age: item.Age,
                                    gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : '',
                                    position: item.Position,
                                    team_name: item.Team_Name
                                });
                            });
                            $('#members-list').find('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                            vueTeamsList.hasMore = false;
                        }
                        catch (e) {
                            $("#body-content").html(response.data);
                        }
                    }
                })
                .catch(error => {
                    $("#body-content").html(error.response);
                });

        }
    }
});


var vueMembersList = new Vue({
    el: "#members-list",
    data: {
        last_id: 0,
        num: 10,
        url: '../api/GetUsersByOrgId',
        lists: [],
        nextToDoId: 0,
        hasMore: true,
        url_profile: '/UserProfile/Index'
    },
    mounted() {
        this.LoadMore();
    },
    computed: {

    },
    methods: {
        LoadMore: function () {
            if (this.hasMore) {
                $('#members-list').find('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');

                axios.get(this.url + '/' + org_menu.org_id + '/' + this.num + '/' + this.last_id)
                    .then(response => {
                        var str = response.data.toString();
                        if (str.indexOf("<!--This is the login layout-->") == 0) {
                            alert("The system detects that you have not operated for a long time, please login again");
                            document.clear();
                            location.reload();
                        }
                        else {
                            try {
                                var json = JSON.parse(response.data);
                                $.each(json, function (index, item) {
                                    vueMembersList.lists.push({
                                        id: vueTeamsList.nextTodoId++,
                                        uid: item.UId,
                                        img_path: item.Img_Path,
                                        name: item.Name,
                                        age: item.Age,
                                        gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : '',
                                        position: item.Position,
                                        team_name: item.Team_Name
                                    });
                                    vueMembersList.last_id = (item.List_Id > vueMembersList.last_id) ? item.List_Id : vueMembersList.last_id;
                                });

                                if (json.length < 10) {
                                    $('#members-list').find('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                                    vueMembersList.hasMore = false;
                                }
                                else {
                                    $('#members-list').find('#more').removeAttr('disabled').removeClass('text-muted').addClass('text-white').html("More...");
                                    vueMembersList.hasMore = true;
                                }
                            }
                            catch (e) {
                                $("#body-content").html(response.data);
                            }
                        }
                    })
                    .catch(error => {
                        $("#body-content").html(error.response);
                    });
            }
        },
        ToProfile: function (user_id) {
            $('#loading-panel').removeAttr('hidden');

            window.location.href = "/UserProfile/Index?user_id=" + user_id; 

            $('#loading-panel').attr('hidden', 'hidden');
        }
    }
});