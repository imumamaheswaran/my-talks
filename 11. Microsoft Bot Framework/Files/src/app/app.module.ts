import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';

// Third Party
import { GridModule } from '@progress/kendo-angular-grid';


import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Core components
import { BackdropsComponent } from './core/elements/backdrops';
import { Jumbotron1Component } from './core/elements/jumbotron-1';
import { Jumbotron2Component } from './core/elements/jumbotron-2';
import { LeftSidebar1Component } from './core/elements/left-sidebar-1';
import { Navbar1Component } from './core/elements/navbar-1';
import { RightSidebar1Component } from './core/elements/right-sidebar-1';
import { TopNavigation1Component } from './core/elements/top-navigation-1';
import { TopNavigation2Component } from './core/elements/top-navigation-2';

//App Components

import { DashboardComponent } from './dashboard/dashboard.component';
import { ActivitiesComponent } from './activities/activities.component';
import { ActivitiesTableComponent } from './activities/activities-table.component';
import { ActivitiesService } from './activities/activities.service';
import { EventChartDirective } from './activities/event-chart';
//Pipes
import { FromNowPipe } from './shared/pipes/from-now.pipe';
import { KeysPipe } from './shared/pipes/keys.pipe';
import { EnumKeys } from './shared/pipes/enum-keys.pipe';
import { InterfaceConfigComponent } from './interface-config/interface-config.component';
import { InterfaceConfigService } from './interface-config/interface-config.service';

@NgModule({
  declarations: [
    AppComponent,
    BackdropsComponent,
    LeftSidebar1Component,
    Navbar1Component,
    RightSidebar1Component,
    DashboardComponent,
    ActivitiesComponent,
    ActivitiesTableComponent,
    EventChartDirective,
    FromNowPipe,
    KeysPipe,
    EnumKeys,
    InterfaceConfigComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    GridModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: InterfaceConfigComponent },
      { path: 'apps/activities', component: ActivitiesComponent },
      { path: 'apps/interface-config', component: InterfaceConfigComponent },
      { path: '**', component: InterfaceConfigComponent },
    ])
  ],
  providers: [ActivitiesService, InterfaceConfigService],
  bootstrap: [AppComponent]
})
export class AppModule { }
