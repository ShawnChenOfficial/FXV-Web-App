var setgender = new Vue({
        el: "#SetGender",
        data() {
            return {
                gender: pageData.gender,
                isMale: pageData.gender == "Male"
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
                isPublic: pageData.isPublic
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
                isReverseOrder: pageData.isReverseOrder
            }
        },
        methods: {
            Setter: function (value) {
                setscoreorder.isReverseOrder = value;
            }
        }
    })