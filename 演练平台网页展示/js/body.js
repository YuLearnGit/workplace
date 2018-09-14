// 初始进入无数据显示chart1中路径option11
var option11 = {
	grid: {
		top: 10,
		left: 10,
		right: 10,
		bottom: 10
	},
	xAxis: {
		splitLine: {
			show: false
		},
		axisLine: {
			show: false
		},
		axisTick: {
			show: false
		},
		axisLabel: {
			show: false
		},
		max: 1000,
		min: 0
	},
	yAxis: {
		silent: true,
		splitLine: {
			show: false
		},
		axisLine: {
			show: false
		},
		axisTick: {
			show: false
		},
		axisLabel: {
			show: false
		},
		max: 1000,
		min: 0
	},
	series: [{
		coordinateSystem: 'cartesian2d',
		type: 'lines',
		polyline: true,
		zlevel: 2,
		effect: {
			show: true,
			constantSpeed: 150,
			//			period: 3,
			trailLength: 0.1,
			symbolSize: 0,
			symbol: 'arrow',
			//						symbol: 'image://../asset/img/echarts-logo.png',
			loop: true
		},
		lineStyle: {
			normal: {
				width: 3,
				color: '#00ffff',
				opacity: 0.1,
				curveness: 0.2
			}
		},
		data: [{
			coords: [
				[100, 500],
				[500, 500]
			]
		}, {
			coords: [
				[500, 500],
				[900, 700]
			]

		}, {
			coords: [
				[500, 500],
				[900, 220]
			]
		}]
	}]
};

//点击启动按钮后在chart1中添加数据option12
//在攻击状态下开启防御关闭防御后回复到最初的移动路径option12
var option12 = {
	series: [{
		effect: {
			symbolSize: 8,
		},
		data: [{
			coords: [
				[100, 500],
				[500, 500]
			]
		}, {
			coords: [
				[500, 500],
				[900, 700]
			]

		}, {
			coords: [
				[500, 500],
				[900, 220]
			]
		}]
	}]
}

//在没开启防御下点击第三方攻击在chart2添加数据option21
var option21 = {
	grid: {
		top: 10,
		left: 10,
		right: 10,
		bottom: 10
	},
	xAxis: {
		splitLine: {
			show: false
		},
		axisLine: {
			show: false
		},
		axisTick: {
			show: false
		},
		axisLabel: {
			show: false
		},
		max: 1000,
		min: 0
	},
	yAxis: {
		silent: true,
		splitLine: {
			show: false
		},
		axisLine: {
			show: false
		},
		axisTick: {
			show: false
		},
		axisLabel: {
			show: false
		},
		max: 1000,
		min: 0
	},
	series: [{
		coordinateSystem: 'cartesian2d',
		type: 'lines',
		polyline: true,
		zlevel: 2,
		effect: {
			show: true,
			constantSpeed: 200,
			trailLength: 0.02,
			symbolSize: 8,
			symbol: 'arrow',
			//						symbol: 'image://../asset/img/echarts-logo.png',
			loop: true
		},
		lineStyle: {
			normal: {
				width: 3,
				color: 'red',
				opacity: 0.1,
				curveness: 0.9
			}
		},
		data: [{
			coords: [
				[455, 720],
				[500, 500]
			]
		}, {
			coords: [
				[500, 500],
				[900, 700]
			]
		}, {
			coords: [
				[500, 500],
				[900, 220]
			]

		}]
	}]
};

//在没开启防御下点击停止攻击在清空chart2数据option22
//在开启防御下点击停止攻击在清空chart2数据option22
var option22 = {
	series: [{
		data: []
	}]
}

//在开启防御下点击第三方攻击在chart2中改变路径option23
//在第三方攻击下开启防御在chart2中改变路径option23
var option23 = {
	series: [{
		data: [{
			coords: [
				[440, 720],
				[300, 500]
			]
		}]
	}]
};

//在第三方攻击下开启防御在chart1中添加路径option13
var option13 = {
	series: [{
		effect: {
			symbolSize: 5,
		},
		data: [{
			coords: [
				[100, 500],
				[300, 500]
			]
		}, {
			coords: [
				[300, 500],
				[500, 500]
			]

		}, {
			coords: [
				[500, 500],
				[900, 700]
			]

		}, {
			coords: [
				[500, 500],
				[900, 220]
			]

		}]
	}]
};