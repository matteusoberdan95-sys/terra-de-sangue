# Sprites — Inimigos

Mesmo contrato do Arandu: **8 frames horizontais**, cada frame **256×256**, fundo **transparente**, vista lateral beat 'em up.

Normalizar sheets irregulares para **2048×256** (recorte por silhueta, nao por largura fixa).

---

## Onde colocar o ZIP (usuario)

**Pasta de entrega:**

```text
assets/art/sprites/enemies/incoming/
```

1. Copie o `.zip` para essa pasta (qualquer nome, ex: `mercenary.zip`).
2. Avise no chat.
3. O agente descompacta, identifica as animacoes, normaliza, renomeia e move para `assets/art/sprites/enemies/`.

**Destino final dos PNGs (apos processamento):**

```text
assets/art/sprites/enemies/
  mercenary_idle_sheet.png
  mercenary_walk_sheet.png
  mercenary_attack_sheet.png
  mercenary_hit_sheet.png
```

Ferramenta de normalizacao: `tools/normalize_sprite_sheet.py`

---

## Mercenario — **integrado**

Sheets em `assets/art/sprites/enemies/mercenary_*.png`. Codigo: `MercenarySpriteArt.cs`.

| Arquivo | Animacao | Status |
|---------|----------|--------|
| `mercenary_idle_sheet.png` | idle | integrado |
| `mercenary_walk_sheet.png` | walk | integrado |
| `mercenary_attack_sheet.png` | attack | integrado |
| `mercenary_hit_sheet.png` | hit | integrado |
| `mercenary_death_sheet.png` | death | opcional (fase gore) |

---

## Bruto — proximo (elite)

Executor com corrente. Mais alto e largo que mercenario. Referencia de escala: **mercenary_walk_sheet.png**.

| Arquivo | Animacao |
|---------|----------|
| `brute_idle_sheet.png` | idle |
| `brute_walk_sheet.png` | walk |
| `brute_attack_sheet.png` | attack (corrente/clava) |
| `brute_hit_sheet.png` | hit |

Codigo pronto: `BruteSpriteArt.cs` (fallback pixel ate PNGs existirem).

### Prompt — brute_idle_sheet

```text
[Prompt base acima]

CHARACTER: Breu-Ferro brute, chain executioner, taller and wider than mercenary,
massive shoulders, partial face wrap, heavy chain wrapped on one arm,
wood club or iron hook, muddy boots, brutal silent enforcer.

ANIMATION: idle 8 frames, heavy breathing, chain sways, menacing stillness.

Side view facing RIGHT. Same scale as attached mercenary reference.
```

### Prompt — brute_walk_sheet / attack / hit

Mesmo personagem do idle. Walk: passos pesados, chain balanca. Attack: golpe de corrente ou clava. Hit: recuo com dor, nao morre ainda.

---

## Sargento do Ferro — mini-chefe Aldeia

Arquivos com prefixo `sergeant_`. Referencia: mercenario ou brute walk.

| Arquivo | Animacao |
|---------|----------|
| `sergeant_idle_sheet.png` | idle |
| `sergeant_walk_sheet.png` | walk |
| `sergeant_attack_sheet.png` | attack (sabre) |
| `sergeant_hit_sheet.png` | hit |

Codigo pronto: `MiniBossSpriteArt.cs`.

### Prompt — sergeant_idle_sheet

```text
[Prompt base acima]

CHARACTER: Iron Sergeant of Breu-Ferro Company, mini-boss officer, old military coat,
saber at hip, crushed hat with dull plume, shoulder pauldron, scarred face,
commands retreat but fights dirty.

ANIMATION: idle 8 frames, hand on saber, coat shifts, arrogant posture.

Side view facing RIGHT. Slightly taller than mercenary, same camera angle.
```

---

## Capitao do Ferro — chefe

Arquivos com prefixo `captain_`. Referencia: sergeant ou mercenario walk.

| Arquivo | Animacao |
|---------|----------|
| `captain_idle_sheet.png` | idle |
| `captain_walk_sheet.png` | walk |
| `captain_attack_sheet.png` | attack (correntes + ferro) |
| `captain_hit_sheet.png` | hit |

Codigo pronto: `IronCaptainSpriteArt.cs`.

### Prompt — captain_idle_sheet

```text
[Prompt base acima]

CHARACTER: Iron Captain, Breu-Ferro boss, improvised armor plates, chains as weapon,
half face burned by black oil, heavy cape, scars, cruel commander silhouette.

ANIMATION: idle 8 frames, chains clink, burned side hidden in shadow, dominant stance.

Side view facing RIGHT. Largest human enemy, still same beat em up camera scale.
```

---

## Mercenario — prompts originais (referencia)

## Prompt base (copiar em todas)

```text
2D side-view beat em up game sprite sheet, dark brazilian mythic setting,
1990s arcade proportions, modern painterly finish (NOT chibi, NOT 3D),
transparent PNG background, 8 animation frames in one horizontal row,
each frame exactly 256 pixels wide and 256 pixels tall,
total image size 2048x256 pixels,
character must match the scale and camera angle of the attached reference warrior,
high silhouette readability at small size, dirty worn look, no text, no watermark
```

---

## Prompt — mercenary_idle_sheet

```text
[Prompt base acima]

CHARACTER: colonial-era mercenary for the Breu-Ferro Company, poor violent
hired gun, NOT a glamorous soldier. Short musket on strap, machete on belt,
crushed leather hat, torn dark cape, iron-gray scraps on shoulders, muddy boots,
unshaven tired face, fearful cruel posture.

ANIMATION: idle breathing cycle, 8 frames, subtle shift of weight, hand near weapon,
eyes scanning, cape and strap move slightly.

PALETTE: iron gray #5f6970, dark leather #3a2a22, warm dull skin, blood red accents
only on small cloth scrap, muddy ground tones on boots.

MOOD: brutal, cowardly, worn-out killer. Same visual quality as a dark arcade
Final Fight enemy. Side view facing RIGHT.
```

---

## Prompt — mercenary_walk_sheet

```text
[Prompt base acima]

CHARACTER: same mercenary as idle sheet — identical face, clothes, weapons, proportions,
same Breu-Ferro mercenary design.

ANIMATION: walk cycle 8 frames, heavy boots, shoulders sway, musket bounces on back,
machete handle visible at hip, aggressive forward march.

Side view facing RIGHT. Same scale as reference warrior.
```

---

## Prompt — mercenary_attack_sheet

```text
[Prompt base acima]

CHARACTER: same mercenary — identical design to idle and walk sheets.

ANIMATION: melee attack 8 frames, draws machete and slashes forward OR rifle butt strike,
clear wind-up, strike, recovery. Readable attack silhouette for a beat em up game.

Side view facing RIGHT. Weapon must stay readable in every frame.
```

---

## Prompt — mercenary_hit_sheet

```text
[Prompt base acima]

CHARACTER: same mercenary — identical design.

ANIMATION: hit reaction 8 frames, staggers backward, pain expression, weapon slips,
body bends from impact, does NOT fall dead yet — hurt stun only.

Side view facing RIGHT.
```

---

## Checklist pos-geracao

- [ ] Fundo 100% transparente
- [ ] 8 frames visiveis em linha
- [ ] Mesmo personagem nas 4 animacoes (mesmo chapéu, arma, roupa)
- [ ] Escala parecida com Arandu (nao gigante, nao anao)
- [ ] Sem texto na imagem
- [ ] Salvar em `assets/art/sprites/enemies/`
- [ ] Avisar no chat para integrar em `MercenarySpriteArt.cs`

---

## Integracao

Todos os inimigos usam `ExternalSpriteSheetArt.cs` + `*SpriteArt.cs`:

- carregar sheets externos se existirem;
- fallback pixel C# se nao existirem;
- scale 0.27 e offset em `EnemyBase.AttachPixelSprite()` — nao compensar sheet ruim no codigo.

Ver `docs/16_personagens_e_faccoes.md` e `docs/07_handoff_codex_cursor.md`.
