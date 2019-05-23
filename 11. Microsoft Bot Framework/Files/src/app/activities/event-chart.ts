import { Directive, ElementRef, HostListener, Input, OnInit, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';

import { IActivity } from './models/activity.model';
import { IStage } from './models/stage.model';
import { ActivityStatus } from './models/activity-status.model';
import { IMessageArchive } from './models/message-archive.model';

import * as go from 'gojs';

@Directive({
    selector: '[eventChart]'
})
export class EventChartDirective implements OnInit, OnChanges {
    private myDiagram: go.Diagram;

    @Input() selectedActivity: IActivity;

    @Input()
    selectedStage: IStage;

    @Output()
    private selectedStageChange = new EventEmitter<IStage>();

    constructor(private element: ElementRef) {
    }

    ngOnInit(): void {
        // this.drawChart();
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.selectedActivity && changes.selectedActivity.currentValue) {
            this.selectedActivity = changes.selectedActivity.currentValue as IActivity;
            this.drawChart();
        }
        if (changes.selectedStage) {
            this.drawChart();
        }
    }

    drawChart(): void {
        const $ = go.GraphObject.make;
        //resetting the diagram div if diagram already present
        if (this.myDiagram)
            this.myDiagram.div = null;
        this.myDiagram = $(go.Diagram, this.element.nativeElement,
            {
                initialContentAlignment: go.Spot.Center,  // center the content
                "undoManager.isEnabled": true,  // enable undo & redo
                layout: $(go.TreeLayout,
                    { angle: 0, nodeSpacing: 10, layerSpacing: 30 }),
                allowHorizontalScroll: true
            });

        function geoFunc(archives) {
            if (archives && archives.length > 0) {
                return go.Geometry.parse("M27.879 5.879l-3.757-3.757c-1.167-1.167-3.471-2.121-5.121-2.121h-14c-1.65 0-3 1.35-3 3v26c0 1.65 1.35 3 3 3h22c1.65 0 3-1.35 3-3v-18c0-1.65-0.955-3.955-2.121-5.121zM20 4.236c0.069 0.025 0.139 0.053 0.211 0.082 0.564 0.234 0.956 0.505 1.082 0.631l3.757 3.757c0.126 0.126 0.397 0.517 0.631 1.082 0.030 0.072 0.057 0.143 0.082 0.211h-5.764v-5.764zM26 28h-20v-24h12v8h8v16zM8 16h16v2h-16zM8 20h16v2h-16zM8 24h16v2h-16z", true);
            }
            return go.Geometry.parse("", true);

        }

        // define a simple Node template
        this.myDiagram.nodeTemplate =
            $(go.Node, "Auto",  // the Shape will go around the TextBlock
                $(go.Shape, "Circle",
                    {
                        strokeWidth: 3,
                        margin:0,
                        height: 100,
                        width: 100,
                    },
                    new go.Binding("fill", "color"),
                    new go.Binding("stroke", "strokeColor")
                ),
                new go.Binding("location", "loc", go.Point.parse),
                $(go.TextBlock,
                    {
                        margin: 0,
                        font: "0.5em"
                    },
                    new go.Binding("text", "artifact")
                )
                ,
                $(go.Shape,
                    {
                        margin: 3,
                        fill: "transparent",
                        strokeWidth: 0

                    },
                    new go.Binding("geometry", "archives", geoFunc)
                )
            );


        this.myDiagram.autoScrollRegion = 0;
        this.myDiagram.allowVerticalScroll = false;

        // but use the default Link template, by not setting Diagram.linkTemplate

        // create the model data that will be represented by Nodes and Links

        let nodeDataArrayProperty = [];

        let x = 0, y = 50;
        var keyList = [];
        if (!(this.selectedActivity.stages && this.selectedActivity.stages.length > 0))
            return;

        for (let i = 0; i < this.selectedActivity.stages.length; i++) {
            let color = "", strokeColor = "";

            if (this.selectedActivity.stages[i].status === 1) {
                color = "#68c46d";
                strokeColor = "#388e3c";
            }
            else if (this.selectedActivity.stages[i].status === 2) {
                color = "#ffc666";
                strokeColor = "#ffa000";
            }
            else if (this.selectedActivity.stages[i].status === 3) {
                color = "#e55454";
                strokeColor = "#b71c1c";
            }
            else {
                color = "#64a9ed";
                strokeColor = "#1976d2";
            }

            if (keyList.indexOf(this.selectedActivity.stages[i].id) === -1) {
                keyList.push(this.selectedActivity.stages[i].id)
                nodeDataArrayProperty.push({
                    id: i,
                    key: this.selectedActivity.stages[i].id,
                    artifact: this.selectedActivity.stages[i].currentStage,
                    archives: this.selectedActivity.stages[i].archives ? this.selectedActivity.stages[i].archives : [],
                    color: color,
                    strokeColor: strokeColor,
                    //loc: [x, y].join(" ")
                });

                x += 100;
            }

        }

        let linkDataArrayProperty = [];

        for (let i = 0; i < nodeDataArrayProperty.length - 1; i++) {
            linkDataArrayProperty[i] = {
                from: nodeDataArrayProperty[i].key,
                to: nodeDataArrayProperty[i + 1].key
            }
        }

        this.myDiagram.model = new go.GraphLinksModel(
            nodeDataArrayProperty, linkDataArrayProperty
        );

        this.myDiagram.addDiagramListener("ObjectSingleClicked", e => {
            let part = e.subject.part;
            if (part instanceof go.GraphObject) {
                this.selectedStageChange.emit(this.selectedActivity.stages.find(q => q.id === part.data.key));
            }
        })
    }


}