
# WIP!

![Image with impressions from the presentation slides](./img_header_coll2.png)

<h1>Combining Eye-Tracking and EEG Experiments with immersive Virtual Reality </h1> 
<h2>Challenges and Opportunities for Cognitive Neuroscientists — a workshop </h2>
 
<br/>

[![version](https://img.shields.io/badge/version-2024.2-yellow.svg?maxAge=259200)](#)

<h2>Abstract</h2>

Cognitive scientists are increasingly keen to incorporate immersive Virtual Reality (VR) into their tool stack for conducting experiments. In this interactive workshop, we will explore together the current state of VR technology from an experimenter’s perspective. A key part of our workshop will be dedicated to the integration of techniques such as EEG and eye tracking, which are widely used in cognitive neuroscience, into VR-based experiments. We will illustrate an exemplary workflow of extending an existing experiment, originally designed for a traditional 2D screen, into a version which makes use of immersive VR. In the first section, we will conceptually navigate through various challenges encountered during this process and discuss the advantages and particularities of the currently most common VR headsets. We will discuss the trade-off between maintaining experimental control and achieving naturalism, spatial reference frames, managing timing aspects, and explore methodologies to incorporate the fact that data was captured in VR into the analysis of EEG and eye tracking data. In the second part of the workshop, we encourage participants keen on exploring the hands-on side of VR experimentation to join us in programming a simple demo experiment using the Unity game engine. This practical exercise will provide insights into how some of the theoretical concepts discussed earlier are applied in real-world implementation.

Our goal is to offer a comprehensive, realistic overview of the process involved in establishing a VR-based experiment from the perspective of a cognitive neuroscientist, thereby enhancing understanding and intuition about whether VR integration represents a valuable option for specific research scenarios.


<h2>Instructions</h2>

> **Shortcuts**  
Code relevant for the ECG (pre)processing can be found in [/Code/Analyses/VRTask/Cardio/Preprocessing](/Code/Analyses/VRTask/Cardio/Preprocessing).  
Most of the main statistical analysis are performed in [/Code/Analyses/VRTask/VRCC_main_analysis.Rmd](/Code/Analyses/VRTask/VRCC_main_analysis.Rmd), further notebooks with exploratory analyses and the preprocessing can be found in [/Code/Analyses/VRTask](/Code/Analyses/VRTask).  
All relevant subfolders contain separate README files with concrete explanations.

> **Most important**  
If you run into problems, please do not hesitate to contact us (e.g., via email) or open an issue here. So if you have questions or want to work with the code or the slides, I am happy to support you.
  
**How to get started:**   
1. Download the data set from [Edmond – The Open Research Data Repository of the Max Planck Society](https://doi.org/10.17617/3.KJGEZQ)  
    There are data-readme files on Edmond which explain what the single folders and files contain.
2. Clone this repository to a clean local directory. 
3. Replace the `\Data` folder with the actual data which you downloaded in step 1 and unzipped. 
4. Now you should be ready to go. 😊



<h2>Versions</h2>  

> You can use the `tags` in the repo to identify the according commits.


###### v0.1
`2024-01`: Code associated with the preprint:
* <a href="">  Klotzsche*, Motyka*, Molak, Sahula, Darmová, Byrnes, Fajnerová, Gaebler, <i>bioRxiv</i>, 2024</a>
