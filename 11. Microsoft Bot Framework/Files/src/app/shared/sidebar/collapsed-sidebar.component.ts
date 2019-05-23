import { Component } from '@angular/core';
import { Route, ActivatedRoute, Router } from "@angular/router";

@Component({
    selector: 'collapsed-sidebar',
    template: `<h1>Test</h1>`
})
export class CollapsedSidebarComponent {
    children: { path: string, name: string }[];

    constructor(private router: Router, private activatedRouter: ActivatedRoute) {
        var i = 0;
        console.log(i);
        console.log(this.activatedRouter.parent.children);
    }
}