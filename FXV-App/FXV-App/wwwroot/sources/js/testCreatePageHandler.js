var setgender = new Vue({
    el: "#SetGender",
    data() {
        return {
            gender: "Male",
            isMale: true
        }
    },
    methods: {
        Setter: function (value) {
            setgender.gender = value;
            setgender.isMale = value == "Male";
        }
    }
})

var setvisibility = new Vue({
    el: "#SetVisibility",
    data() {
        return {
            isPublic: true
        }
    },
    methods: {
        Setter: function (value) {
            setvisibility.isPublic = value;
        }
    }
})

var setscoreorder = new Vue({
    el: "#SetScoreOrder",
    data() {
        return {
            isReverseOrder: false
        }
    },
    methods: {
        Setter: function (value) {
            setscoreorder.isReverseOrder = value;
        }
    }
})