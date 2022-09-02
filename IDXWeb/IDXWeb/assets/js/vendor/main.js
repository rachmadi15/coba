import * as _ from './utils'
import activeStateMobile from './activeStateMobile'
import WPViewportFix from './windowsPhoneViewportFix'
import objectFitPolyfill from './objectFitPolyfill'
import slider from './slider'
import chart from './chart'
import tabMenu from './tabMenu'
import navMenu from './navMenu'
import datePicker from './datePicker'
import accordion from './accordion'
import filterDisplay from './filterDisplay'
import scrollToTop from './scrollToTop'
import selectInput from './selectInput'
import clamp from './clamp'
import calendarClose from './calendarClose'
import livetime from './livetime'
import countdown from './countdown'
import dropdownToggle from './dropdownToggle'
import dataTables from './dataTables'
import tooltip from './tooltip'

const App = {
    activeStateMobile,
    WPViewportFix,
    objectFitPolyfill,
    ...slider,
    ...chart,
    ...tabMenu,
    ...navMenu,
    ...datePicker,
    ...accordion,
    ...filterDisplay,
    ...scrollToTop,
    selectInput,
    clamp,
    ...calendarClose,
    livetime,
    countdown,
    ...dropdownToggle,
    ...dataTables,
    ...tooltip,
}

for (let fn in App) {
    App[fn]()
}

export default App
