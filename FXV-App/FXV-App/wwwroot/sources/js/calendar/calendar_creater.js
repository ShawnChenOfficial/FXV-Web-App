var today;
'use strict';

const Heading = ({ date, changeMonth, resetDate }) =>
    React.createElement("nav", { className: "calendar--nav" },
        React.createElement("a", { onClick: () => changeMonth(date.month() - 1) }, "\u2039"),
        React.createElement("h1", { onClick: () => resetDate() }, date.format('MMMM'), " ", React.createElement("small", null, date.format('YYYY'))),
        React.createElement("a", { onClick: () => changeMonth(date.month() + 1) }, "\u203A"));



const Day = ({ currentDate, date, startDate, endDate, onClick }) => {
    today = currentDate.toString();
    let className = [];

    if (moment().isSame(date, 'day')) {
        className.push('active');
        className.push('dot');
    }

    if (date.isSame(startDate, 'day')) {
        className.push('start');
    }

    if (!date.isSame(currentDate, 'month')) {
        className.push('muted');
    }

    return (
        React.createElement("span", { onClick: () => onClick(date), currentDate: date, className: className.join(' ') }, date.date()));

    //add the small dot right here.

};

const Days = ({ date, startDate, endDate, onClick }) => {
    const thisDate = moment(date);
    const daysInMonth = moment(date).daysInMonth();
    const firstDayDate = moment(date).startOf('month');
    const previousMonth = moment(date).subtract(1, 'month');
    const previousMonthDays = previousMonth.daysInMonth();
    const nextsMonth = moment(date).add(1, 'month');
    let days = [];
    let labels = [];

    for (let i = 1; i <= 7; i++) {
        if (window.CP.shouldStopExecution(0)) break;
        labels.push(React.createElement("span", { className: "label" }, moment().day(i).format('ddd')));
    } window.CP.exitedLoop(0);

    for (let i = firstDayDate.day(); i > 1; i--) {
        if (window.CP.shouldStopExecution(1)) break;
        previousMonth.date(previousMonthDays - i + 2);

        days.push(
            React.createElement(Day, { key: moment(previousMonth).format('DD MM YYYY'), onClick: date => onClick(date), currentDate: date, date: moment(previousMonth), startDate: startDate, endDate: endDate }));

    } window.CP.exitedLoop(1);

    for (let i = 1; i <= daysInMonth; i++) {
        if (window.CP.shouldStopExecution(2)) break;
        thisDate.date(i);

        days.push(
            React.createElement(Day, { key: moment(thisDate).format('DD MM YYYY'), onClick: date => onClick(date), currentDate: date, date: moment(thisDate), startDate: startDate, endDate: endDate }));

    } window.CP.exitedLoop(2);

    const daysCount = days.length;
    for (let i = 1; i <= 42 - daysCount; i++) {
        if (window.CP.shouldStopExecution(3)) break;
        nextsMonth.date(i);
        days.push(
            React.createElement(Day, { key: moment(nextsMonth).format('DD MM YYYY'), onClick: date => onClick(date), currentDate: date, date: moment(nextsMonth), startDate: startDate, endDate: endDate }));

    } window.CP.exitedLoop(3);

    return (
        React.createElement("nav", { className: "calendar--days" },
            labels.concat(),
            days.concat()));


};

class Calendar extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            date: moment(),
            startDate: moment()
        };

    }

    resetDate() {
        this.setState({
            date: moment()
        });

    }

    changeMonth(month) {
        const { date } = this.state;

        date.month(month);

        this.setState(
            date);

    }


    changeDate(date) {
        let { startDate, endDate } = this.state;

        if (startDate === null || date.isBefore(startDate, 'day') || !startDate.isSame(endDate, 'day')) {
            startDate = moment(date);

            var x = startDate.toString();
            getActicities(x);

        } else if (date.isSame(startDate, 'day') && date.isSame(endDate, 'day')) {
            startDate = null;
            endDate = null;
        } else if (date.isAfter(startDate, 'day')) {
            endDate = moment(date);
        }

        this.setState({
            startDate,
            endDate
        });

    }


    componentDidMount() {
        getActicities(today);
    }

    render() {
        const { date, startDate, endDate } = this.state;

        return (
            React.createElement("div", { className: "calendar" },
                React.createElement(Heading, { date: date, changeMonth: month => this.changeMonth(month), resetDate: () => this.resetDate() }),
                React.createElement(Days, { onClick: date => this.changeDate(date), date: date, startDate: startDate, endDate: endDate })));


    }
}


ReactDOM.render(
    React.createElement(Calendar, null),
    document.getElementById('calendar'));


function getActicities(date) {


    $("#signed_activity").html('<h3 class= "medium-bold-500 card-title text-center text-white">Signed Activities</h3><div class="card-body col-12 text-center" style="padding-top:170px;height:400px"><h4>Loading...</h4><img style="width:2rem;height:2rem" alt="" src="/sources/img/loading.gif"/></div>');
    $("#unsigned_activity").html('<h3 class= "medium-bold-500 card-title text-center text-white">Signed Activities</h3><div class="card-body col-12 text-center" style="padding-top:170px;height:400px"><h4>Loading...</h4><img style="width:2rem;height:2rem" alt="" src="/sources/img/loading.gif"/></div>');

    $.ajax({
        url: '../home/GetActicitiesSigned',
        data: { date },
        type: 'POST',
        cache: false,
        dataType: 'html',
        success: function (data) {
            var str = data.toString();
            if (str.indexOf("<!--This is the login layout-->") == 0) {
                alert("The system detects that you have not operated for a long time, please login again");
                document.clear();
                location.reload();
            }
            else {
                $("#signed_activity").html("");
                $("#signed_activity").html(data);
            }
        },
        error: function (xhr) {
            $("#signed_activity").html(xhr.responseText);
        }

    });

    $.ajax({
        url: '../home/GetActicitiesUnsigned',
        data: { date },
        type: 'POST',
        cache: false,
        dataType: 'html',
        success: function (data) {
            var str = data.toString();
            if (str.indexOf("<!--This is the login layout-->") == 0) {
                alert("The system detects that you have not operated for a long time, please login again");
                document.clear();
                location.reload();
            }
            else {
                $("#unsigned_activity").html("");
                $("#unsigned_activity").html(data);
            }
        },
        error: function (xhr) {
            $("#unsigned_activity").html(xhr.responseText);
        }

    });

}