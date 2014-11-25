Colters
=======
Aries 2014 Student Project
=======

Okay, les enfants, on va essayer de mettre de l'ordre dans le dossier Project Unity, afin qu'on évite de se mélanger les couilles.

=================================================
AVANT DE COMMENCER HYPER IMPORTANT WARING WARING OUHLALA:
=================================================
Certains fichiers comme les fichiers d'images (PNG, et tout), sont liés à Unity, une fois importé, par leurs noms. Pour éviter de perdre et refaire à la main les liens des texutres aux matériaux et ce genre de conneries, veillez à  bien choisir le nom d'une texture au moment de l'import(en respectant la nomenclature).

========================
PREFACE: La nomenclature
========================

Bon, rien de bien sorcier, quand vous créez ou importez un fichier, veillez à ce que ça nomenclature dise un maximum sur lui, sans pour autant faire des noms à rallonge imbitable.
Juste, mettez des trucs genre (Catégorie_nom.extension).
Genre, imaginons une texture d'herbe qu'on utilise seulement en placeHolder, le fichier devrait s'appeler Ground_grass.png, et se trouverait dans un sous-dossier placeHolder. C'est simple, mais ça évite de se retrouver avec un dossier contenant 50 000 fichiers s'appelant tous 045qsd4321x_3qd.png ou des conneries du genre.

====================
I. Le dossier assets
====================

Le dossier principal, évidemment, qui contient tout ce qui a de plus important.

Pour l'instant, le dossier se divise en autre sous dossiers:

-Animators
-Graphical Assets
-Plugins
-Prefabs
-Scenes
-Scripts
-XInputDotNet

Voyons ces dossiers un par un, dans un ordre de bas niveau à haut niveau (plus ou moins... voire pas du tout en fait).

==========
II.Scripts
==========

Plutôt self-explicite. On entrepose ici tout les scripts C#.
Pareil pour les sous-catégories, chaque dossier est étiquetté de façon assez logique et explicite.
"Misc_" contient tout les scripts qui ne rentrent pas dans les autres catégories. Si ce dernier dossier commence à déborder, c'est qu'il serait sans doute temps de créer un nouveau script.

============
III. Prefabs
============

Pour ce qui est des prefabs, c'est plutôt la même chose au niveau de l'orga. Attention, tripoter les réglages des prefabs ou leur noms peut provoquer des bugs, en cas de doute, se référer au tech guy (Nagy, quoi. Moi, quoi.)

==========
IV. SCENES
==========

Bon, je pense que vous savez à quoi sert ce dossier, mais le plus important est de séparer les maps de Test, les maps de Menu et les maps de jeu.
Donc, Dev_Test sert à emmagasiner les maps qui servent uniquement au test de features, Menus sert aux menus (duh !), et bon, après, je pense que vous avez pigé...

==========================
V. Plugins et XInputDotNet
==========================

Ces dossiers vont servir à l'éventuelle implémentation du plugin DirectX XInput qui permet de reconnaître plus naturellement les pads X360. Ca sert pas encore, mais ça devrait.

=============
VI. ANIMATORS
=============

Attention à la confusion, les animators sont les fichiers servant à Unity pour gérer les animations liés à des objets, et non pas les animations elles-mêmes. Elle relèvent donc plus de la gestion du moteur que du boulot des graph'.

=====================
VII. Graphical Assets
=====================

Bon, c'est plutôt clair, là encore, c'est ici qu'on stocke tout les assets graphiques, qu'ils soient bruts de décoffrage ou processé par Unity.
Les objets 3D sont dans meshes, les Textures dans le dossier correspondant, et une fois processé en fichier material, on les stocke dans le dossier Materials. Rien de bien compliqué.

Attention par contre à bien séparer les placeholders des "vrais" fichiers, ça évitera les confusions inutiles.
Puis bien sous-catégoriser le tout, sinon ça va vite devenir du caca. Ne pas hésiter également à rajouter des préfixes aux fichiers histoire de s'y retrouver le mieux possible (Genre, toujours commencer les textures de sols par ground_XXX).
