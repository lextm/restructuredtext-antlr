grammar ReStructuredText;

// Copyright (C) 2011 Bart Kiers
// Copyright (C) 2017 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

options {
  language=CSharp;
}

parse
  :  (element | empty_line)+? EOF
  ;

element
  :  section | sectionElement
  ;
  
sectionElement
  :  comment | listItemBullet | listItemEnumerated | paragraph | lineBlock
  ;

comment
  :  Space* Comment Space* (lineNoBreak lines*)?
  ;

paragraph
  :  lines
  ;

section
  :  (LineBreak Section)? title LineBreak? Section (LineBreak)* sectionElement*
  ;

title
  :  LineBreak textStart
  |  LineBreak lineSpecial Space+ (paragraphNoBreak)?
  |  lineNormal
  |  lineStar
  ;

lineBlock
  :  LineBreak lineBlockLine LineBreak? lineBlockLine*
  ;

lineBlockLine
  :  Block Space indentation? span*? starText
  |  Block Space indentation? span+
  ;

listItemBullet
  :  bulletCrossLine
  |  bulletSimple
  |  LineBreak Space* special=(Minus | Plus)
  ;

bulletCrossLine
  : LineBreak Space* bullet Space* (paragraph+)? 
  ;

bulletSimple 
  :  LineBreak Space* bullet Space+ paragraphNoBreak paragraph* 
  ;

bullet
  :  Star 
  |  Minus 
  |  Plus
  ;

listItemEnumerated
  :  LineBreak enumerated=lineSpecial Space+ (paragraphNoBreak paragraph*)?
  ;
  
paragraphNoBreak
  :  lineNoBreak lines*
  ;

lineNoBreak
  :  indentation? spanNoStar span*?
  ;
  
lines
  :  linesStar
  |  linesNormal
  ;

linesNormal
  :  lineNormal (linesStar | linesNormal?)
  ;
  
linesStar
  :  lineStar
  |  lineStar lineNoBreak linesNormal??  
  |  lineStar lineNoBreak linesStar
  ;

lineNormal
  :  LineBreak indentation? span*? spanNoStar
  |  lineSpecial
  ;
  
lineStar
  :  LineBreak indentation? span*? starText
  ;
 
text
  :  textStart textEnd?
  |  Dot
  |  '"' textEnd?
  |  Any textEnd?
  |  '(' textEnd?
  |  ':' textEnd?
  |  '}' textEnd?
  |  ')' textEnd?
  |  '\\' textEnd?
  |  Space text
  ;

lineSpecial
  :  Numbers Dot
  //|  Alphabet Dot
  ;
  
empty_line
  :  LineBreak Space*
  ;

indentation
  :  Space+
  ;

textStart
  :  forcedText (text_fragment forcedText*)*
  |  text_fragment_firstTwo text_fragment*
  |  '/'
  |  '?'
  |  Equal Equal
  |  Alphabet text_fragment*
  ;

forcedText 
  :  '(*)' 
  |  '[*]' 
  |  '\'*\'' 
  |  '\'"*"\''
  ; 

textEnd
  :  textAtoms
  ;
  
textAtoms
  : text_fragment+
  ;

spanNoStar
  :  reference
  |  referenceIn
  |  hyperlinkTarget
  |  hyperlink
  |  hyperlinkDoc
  |  backTickText
  |  quotedLiteral
  |  text
  |  stars
  ;

span
  :  starText
  |  spanNoStar
  ;
  
stars
  :  Star Star Star Star Star+
  ;

quotedLiteral
  : '>' Space lineNoBreak
  ;

text_fragment_firstTwo
  :  (Minus ~(Space | LineBreak))
  |  (Plus ~Space)
  |  (Numbers Dot ~(Space | LineBreak))
  |  (Block ~Space)
  |  ('\\' '"' Star '"' ~'\\')
  |  text_fragment_start text_fragment_start text_fragment
  ;
  
text_fragment_start
  :  Section
  |  SemiColon
  |  EscapeSequence
  |  UnderScore
  |  Numbers
  |  Alphabet
  |  TimeStar
  |  '/'
  |  '#'
  |  '['
  |  ']'
  |  '('
  |  ')'
  |  Colon
  |  Equal
  |  '?'
  |  '<'
  |  '>'
  |  '&'
  |  '"'
  |  Any
  ;

text_fragment
  :  text_fragment_start
  |  Block
  |  Literal
  |  Comment
  |  Space
  |  Dot
  |  Minus
  |  Quote
  |  (Star Space)
  ;

starText
  :  Star+ starNoSpace starAtoms (LineBreak Star* starNoSpace starAtoms)* Star* LineBreak
  |  Star+ starNoSpace starAtoms Star* LineBreak
//  |  Star ~(Star | LineBreak)
  ;

starAtoms
  :  starAtom* (Star* starAtom)*
  ;

starNoSpace
  :  ~(Star | LineBreak | Space | Section)
  ;

starAtom
  :  ~(Star | LineBreak)
  ;

backTickText
  :  (Colon titled=Alphabet Colon)? body
  ;

body
  :  BackTick BackTick* backTickAtoms BackTick+
  |  BackTick backTickNoSpace backTickAtoms BackTick+
  |  BackTick BackTick
  ;

backTickAtoms
  :  backTickAtom+
  ;

backTickNoSpace
  :  ~(BackTick | LineBreak | Space)
  ;

backTickAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

referenceIn
  :  UnderScore hyperlinkAtom+ Colon Space url
  ;

hyperlinkTarget
  :  UnderScore Any+
  ;
  
hyperlink
  :  BackTick hyperlinkAtom+ Space '<' url '>' BackTick UnderScore
  ;
 
hyperlinkDoc
  :  ':doc:' BackTick hyperlinkAtom+ Space '<' url '>' BackTick
  |  ':doc:' BackTick url BackTick
  ;

url
  :  urlAtom+
  ;
  
urlAtom
  :  ~( LineBreak | BackTick )
  ;
  
hyperlinkAtom
  :  ~( LineBreak | '<' | '>' | BackTick | Star )
  ;

Literal
  :  Colon LineBreak LineBreak* Colon Colon
  ;

Section
  :  (Minus | Equal | Plus) (Minus | Equal | Plus) (Minus | Equal | Plus)+
  ;
  
TimeStar
  : Numbers Star
  | 'x' Star
  ;

Alphabet
  : [A-Za-z]+
  ;
  
Numbers
  : [0-9]+
  ;

Quote
  :  Colon Colon
  ;

Dot
  :  '.'
  ;
  
SemiColon
  :  ';'
  ;
  
Colon
  :  ':'
  ;

Equal
  :  '='
  ;

Plus
  :  '+'
  ;

Minus
  :  '-'
  ;

Block
  :  '|'
  ;

Comment
  :  ('.. ' LineBreak?)
  |  ('..' LineBreak)
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | Star)
  ;

LineBreak
  :  '\r'? '\n'
  ;

Any
  :  .
  ;